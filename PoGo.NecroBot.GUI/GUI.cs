using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using POGOProtos.Inventory.Item;
using POGOProtos.Enums;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.Event;
using System.IO;
using POGOProtos.Data;
using GMap.NET.WindowsForms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using PoGo.NecroBot.GUI.Util;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.GUI.Aggregators;
using PoGo.NecroBot.GUI.Tasks;

using System.Text.RegularExpressions;
using PoGo.NecroBot.GUI.Utils;
using PoGo.NecroBot.GUI.UserControls;

namespace PoGo.NecroBot.GUI
{
    public partial class GUI : Form
    {
        private Task discordTask;
        public GUI()
        {
            InitializeComponent();
        }

        private void GUI_Load(object sender, EventArgs e)
        {
            Bot.GUI = this;

            string strCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;

            this.Show();

            string subpath = "";
            string profilePath = "";

            GUILogin loadProfile = new GUILogin();
            var result = loadProfile.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                profilePath = loadProfile._loginProfileFolder;
                subpath = Directory.GetCurrentDirectory() + "\\config\\profiles\\" + loadProfile._loginProfileName;
            }

            Bot.subPath = subpath;

            Translation translations = new Translation();
            translations.Save(Path.Combine(Directory.GetCurrentDirectory() + "\\config\\translations\\", "translation.en.json"));

            Logger.SetLogger(new GUILogger(LogLevel.Info), subpath);

            GlobalSettings _settings = GlobalSettings.Load(profilePath);

            if (_settings == null)
            {
                MessageBox.Show("Error loading profile, restart bot and try again.", "Erreor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            _settings.AutoUpdate = false;
            _settings.AmountOfPokemonToDisplayOnStart = 0;
            _settings.StartupWelcomeDelay = false;
            _settings.isGui = true;
            _settings.UseWebsocket = false;

            if (loadProfile._loginUseGPX)
            {
                _settings.UseGpxPathing = loadProfile._loginUseGPX;
                _settings.GpxFile = loadProfile._loginGPXFile;
            }

            if(loadProfile._loginUseLastCoords)
            {
                _settings.DefaultLatitude = loadProfile._loginLastLat;
                _settings.DefaultLongitude = loadProfile._loginLastLng;
            }

            StateMachine machine = new StateMachine();
            Session session = new Session(new ClientSettings(_settings), new LogicSettings(_settings));
            session.Client.ApiFailure = new ApiFailureStrategy(session);
            Bot._Session = session;
            Bot._Machine = machine;
            Bot.ProfileSettings = loadProfile._profileSettings;
            Bot.ProfileSettings.profilepath = profilePath;
            Bot.GlobalSettings = _settings;

            Bot.InitImageList(session);
            Bot.LoadGUISettings();
        }

        private static void UnhandledExceptionEventHandler(object obj, UnhandledExceptionEventArgs args)
        {
            Logger.Write("Exceptiion caught, writing LogBuffer.", force: true);
            throw new Exception();
        }

        private void checkShowPokemons_CheckedChanged(object sender, EventArgs e)
        {
            Bot.mapOverlays["pokemons"].IsVisibile = checkShowPokemons.Checked ? true : false;
        }

        private void checkShowPokestops_CheckedChanged(object sender, EventArgs e)
        {
            Bot.mapOverlays["pokestops"].IsVisibile = checkShowPokestops.Checked ? true : false;
        }

        private void checkShowPokegyms_CheckedChanged(object sender, EventArgs e)
        {
            Bot.mapOverlays["pokegyms"].IsVisibile = checkShowPokegyms.Checked ? true : false;
        }

        private void checkShowPath_CheckedChanged(object sender, EventArgs e)
        {
            Bot.mapOverlays["path"].IsVisibile = checkShowPath.Checked ? true : false;
        }

        private void cmdSaveSettings_Click(object sender, EventArgs e)
        {
            Bot.SaveGUISettings();
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            Bot.StartBotting();
        }

        private void cmdSnipeList_Click(object sender, EventArgs e)
        {
            ManualSnipePokemon.Execute();
        }

        private void cmdTransferSelected_Click(object sender, EventArgs e)
        {
            ManualTransferPokemon.Execute();
        }

        private void cmdEvolveSelected_Click(object sender, EventArgs e)
        {
            ManualEvolvePokemon.Execute();
        }

        private void cmdPowerupSelected_Click(object sender, EventArgs e)
        {
            ManualPowerUpPokemon.Execute();
        }

        private void checkDoPokestops_CheckedChanged(object sender, EventArgs e)
        {
            Bot._Session.GUISettings.ExecutePokestops = checkDoPokestops.Checked;
        }

        private void checkDoPokemons_CheckedChanged(object sender, EventArgs e)
        {
            Bot._Session.GUISettings.ExecutePokemons = checkDoPokemons.Checked;
        }

        private void bntStartSnipingFeed_Click(object sender, EventArgs e)
        {
            if (Bot.PokemonSnipeFeedActive == true)
            {
                Bot.PokemonSnipeFeedActive = false;
            }
            else
            {
                Bot.PokemonSnipeFeedActive = true;
                discordTask = GetPokemonToSnipe.DiscordWebReader.TryStartDiscordReader();
            }
        }

        private void dataSnipingFeeder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns["dataSnipingFeederColBtnSnipe"] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                MessageBox.Show("We clicked to snipe");
            }
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bot.ProfileSettings.LastLat = Bot._Session.Client.CurrentLatitude;
            Bot.ProfileSettings.LastLng = Bot._Session.Client.CurrentLongitude;
            Bot.ProfileSettings.Save();
        }

        public string StatsPlayerName { get { return textPlayerName.Text; } set { textPlayerName.Text = value; }}
        public string StatsPlayerLevel { get { return textPlayerLevel.Text; } set { textPlayerLevel.Text = value; } }
        public string StatsPlayerStardust { get { return textPlayerStardust.Text; } set { textPlayerStardust.Text = value; } }
        public string StatsPlayerPokecoins { get { return textPlayerPokecoins.Text; } set { textPlayerPokecoins.Text = value; } }
        public int StatsPlayerLevelMaxExp { get { return progressPlayerExpBar.Maximum ; } set { progressPlayerExpBar.Maximum = value; } }
        public int StatsPlayerLevelCurrentExp { get { return progressPlayerExpBar.Value; } set { progressPlayerExpBar.Value = value; } }
        public string StatsPlayerExperienceOverLevelExp { get { return labelPlayerExpOverLevelExp.Text; } set { labelPlayerExpOverLevelExp.Text = value; } }

        public string StatsPlayerExperiencePerhour { get { return labelPlayerExpHr.Text; } set { labelPlayerExpHr.Text = value; } }
        public string StatsPlayerPokemonPerHour { get { return labelPlayerPokeHr.Text; } set { labelPlayerPokeHr.Text = value; } }

        public string Titlebar { get { return this.Text; } set { this.Text = value; } }

        public GMapControl GoogleMap { get { return gMap; } set { gMap = value; } }
        public string LiveMapCurrentLatLng { get { return textCurrentLatLng.Text; } set { textCurrentLatLng.Text = value; } }

        public DataGridView DataGridMyPokemons { get { return dataMyPokemons; } set { dataMyPokemons = value; } }
        public string DataGridMyPokemonTab { get { return tabMyPokemons.Text; } set { tabMyPokemons.Text = value; } }

        public DataGridView DataGridMyItems { get { return dataMyItems; } set { dataMyItems = value; } }
        public string DataGridMyItemsTab { get { return tabMyItems.Text; } set { tabMyItems.Text = value; } }

        public DataGridView DataGridMyCandies { get { return dataMyCandies; } set { dataMyCandies = value; } }
        public string DataGridMyCandiesTab { get { return tabMyCandies.Text; } set { tabMyCandies.Text = value; } }

        public DataGridView DataGridConsole { get { return dataGridConsole; } set { dataGridConsole = value; } }

        public DataGridView DataGridSnipePokemons { get { return dataSnipingFeeder; } set { dataSnipingFeeder = value; } }

        public GlobalSettingsControl GlobalSettingsTab { get { return globalSettingsControl; } set { globalSettingsControl = value; } }
        public ItemSettingsControl ItemSettingsTab { get { return itemSettingsControl; } set { itemSettingsControl = value; } }
        public PokemonSettingsControl PokemonSettingsTab { get { return pokemonSettingsControl; } set { pokemonSettingsControl = value; } }
        public SnipingSettingsControl SnipingSettingsTab { get { return snipingSettingsControl; } set { snipingSettingsControl = value; } }

        public TextBox SnipingTextBox { get { return textPokemonSnipeList; } set { textPokemonSnipeList = value; } }
        public bool SnipeOptionAll { get { return radioSnipeGetAll.Checked; } set { radioSnipeGetAll.Checked = value; } }
        public bool SnipeOptionUseSettings { get { return radioSnipeUseSettings.Checked; } set { radioSnipeUseSettings.Checked = value; } }
    }
}
