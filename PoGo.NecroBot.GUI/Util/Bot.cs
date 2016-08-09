using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using PoGo.NecroBot.GUI.Aggregators;
using PoGo.NecroBot.GUI.Tasks;
using PoGo.NecroBot.GUI.Util;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Tasks;
using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Fort;
using POGOProtos.Map.Pokemon;
using POGOProtos.Settings.Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoGo.NecroBot.GUI.Utils
{
    class Bot
    {
        // Global
        public static GUI GUI { get; set; }
        public static Session _Session;
        public static StateMachine _Machine;

        public static string subPath;

        public static ProfileSettings ProfileSettings { get; set; }
        public static GlobalSettings GlobalSettings { get; set; }
        public static GUISettings GUISettings { get; set; }
        public static Dictionary<string, Bitmap> imagesList = new Dictionary<string, Bitmap>();
        public static Dictionary<string, GMapOverlay> mapOverlays;

        // Profile
        public static string _ProfilePlayerName { get; set; }
        public static TeamColor _ProfilePlayerTeam { get; set; }
        public static int _ProfilePlayerLevel { get; set; }
        public static int _ProfilePlayerMaxBagSpace { get; set; }
        public static int _ProfilePlayerMaxPokemonSpace { get; set; }
        public static int _ProfilePlayerStardust { get; set; }
        public static int _ProfilePlayerPokecoins { get; set; }

        // Stats
        public static long _StatsPrevLevelXp { get; set; }
        public static long _StatsNextLevelXp { get; set; }
        public static long _StatsExperience { get; set; }
        public static int _StatsSessionExperience = 0;
        public static int _StatsSessionPokemon = 0;

        // Inventory
        public static Dictionary<ulong, PokemonData> MyPokemons = new Dictionary<ulong, PokemonData>();
        public static Dictionary<ItemId, int> MyItems = new Dictionary<ItemId, int>();
        public static Dictionary<PokemonFamilyId, int> MyCandies = new Dictionary<PokemonFamilyId, int>();

        // LiveMap
        public static PointLatLng _LiveMapCurrentPosition = new PointLatLng();
        public static PointLatLng _LiveMapLastPosition = new PointLatLng();
        public static bool _LiveMapPositionUpdated = false;

        public static Dictionary<string, FortData> _LiveMapPokeStops = new Dictionary<string, FortData>();
        public static Dictionary<string, FortData> _LiveMapPokeGyms = new Dictionary<string, FortData>();
        public static Dictionary<ulong, MapPokemon> _LiveMapPokemons = new Dictionary<ulong, MapPokemon>();

        // Quick Search
        public static List<PokemonSettings> PokemonSettings = new List<PokemonSettings>();

        // SnipingFeed
        public static bool PokemonSnipeFeedActive = false;
        public static List<Tasks.SniperInfo> PokemonSnipeFeed = new List<Tasks.SniperInfo>();
        public static List<Tasks.SniperInfo> PokemonSnipeFeedDeleted = new List<Tasks.SniperInfo>();

        public async static void StartBotting()
        {
            if (_Session.GUISettings.isStarted == true)
            {
                MessageBox.Show("Bot is already running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GUIStats _guiStats = new GUIStats();
            GUIItems _guiItems = new GUIItems();
            GUIPokemons _guiPokemons = new GUIPokemons();
            GUILiveMap _guiLiveMap = new GUILiveMap();

            //_guiItems.DirtyEvent += () => UpdateMyItems();
            //_guiPokemons.DirtyEvent += () => UpdateMyPokemons();
            //_guiLiveMap.DirtyEvent += () => UpdateLiveMap();

            var listener = new GUIEventListener();
            var statsAggregator = new GUIStatsAggregator(_guiStats);
            var itemsAggregator = new GUIItemsAggregator(_guiItems);
            var pokemonsAggregator = new GUIPokemonsAggregator(_guiPokemons);

            _Session.EventDispatcher.EventReceived += (IEvent evt) => listener.Listen(evt, _Session);
            _Session.EventDispatcher.EventReceived += (IEvent evt) => statsAggregator.Listen(evt, _Session);
            _Session.EventDispatcher.EventReceived += (IEvent evt) => itemsAggregator.Listen(evt, _Session);
            _Session.EventDispatcher.EventReceived += (IEvent evt) => pokemonsAggregator.Listen(evt, _Session);

            _Machine.SetFailureState(new LoginState());

            _Session.Navigation.UpdatePositionEvent +=
                (lat, lng) => _Session.EventDispatcher.Send(new UpdatePositionEvent { Latitude = lat, Longitude = lng });
            _Session.Navigation.UpdatePositionEvent += Navigation_UpdatePositionEvent;

            _Machine.AsyncStart(new VersionCheckState(), _Session);
            if (_Session.LogicSettings.UseSnipeLocationServer)
                SnipePokemonTask.AsyncStart(_Session);

            if (Bot.ProfileSettings.UseLiveMap)
            {
                var livemapAggregator = new GUILiveMapAggregator(_guiLiveMap);
                _Session.EventDispatcher.EventReceived += (IEvent evt) => livemapAggregator.Listen(evt, _Session);

                Bot.GUI.GoogleMap.MapProvider = GoogleMapProvider.Instance;
                GMaps.Instance.Mode = AccessMode.ServerOnly;
                Bot.GUI.GoogleMap.Zoom = 15;

                Bot.mapOverlays = new Dictionary<string, GMapOverlay>();
                Bot.mapOverlays.Add("pokemons", new GMapOverlay("pokemons"));
                Bot.GUI.GoogleMap.Overlays.Add(Bot.mapOverlays["pokemons"]);

                Bot.mapOverlays.Add("pokestops", new GMapOverlay("pokestops"));
                Bot.GUI.GoogleMap.Overlays.Add(Bot.mapOverlays["pokestops"]);

                Bot.mapOverlays.Add("pokegyms", new GMapOverlay("pokegyms"));
                Bot.GUI.GoogleMap.Overlays.Add(Bot.mapOverlays["pokegyms"]);

                Bot.mapOverlays.Add("player", new GMapOverlay("player"));
                Bot.GUI.GoogleMap.Overlays.Add(Bot.mapOverlays["player"]);

                Bot.mapOverlays.Add("path", new GMapOverlay("path"));
                Bot.GUI.GoogleMap.Overlays.Add(Bot.mapOverlays["path"]);

                Bitmap player = new Bitmap(Bot.imagesList["player"]);
                player.MakeTransparent(System.Drawing.Color.White);
                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(_Session.Client.CurrentLatitude, _Session.Client.CurrentLongitude), player);
                Bot.mapOverlays["player"].Markers.Add(marker);
                Bot.GUI.GoogleMap.Position = new PointLatLng(_Session.Client.CurrentLatitude, _Session.Client.CurrentLongitude);

                GUI.Invoke(new Action(() => GUI.LiveMapCurrentLatLng = _Session.Client.CurrentLatitude.ToString() + "," + _Session.Client.CurrentLongitude.ToString()));
            }

            // Set on first start
            _Session.GUISettings.isStarted = true;
        }

        public static void InitImageList(Session session)
        {
            var resourceImages = PoGoImages.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, false);

            if (resourceImages != null)
            {
                foreach (DictionaryEntry entry in resourceImages)
                {
                    var value = entry.Value as Bitmap;
                    if (value != null)
                    {
                        Bitmap bmp = new Bitmap(value, new Size(40, 30));
                        Bot.imagesList.Add((string)entry.Key, bmp);

                    }
                }
            }
        }

        public static void UpdateConsole(LogLevel level, string message, System.Drawing.Color color)
        {
            DataGridViewRow newRow = new DataGridViewRow();

            newRow.CreateCells(Bot.GUI.DataGridConsole, DateTime.Now, level.ToString(), message);

            newRow.DefaultCellStyle.ForeColor = color;
            newRow.DefaultCellStyle.SelectionForeColor = color;

            Bot.GUI.DataGridConsole.Invoke(new Action(() => Bot.GUI.DataGridConsole.Rows.Add(newRow)));
            Bot.GUI.DataGridConsole.Invoke(new Action(() => Bot.GUI.DataGridConsole.FirstDisplayedScrollingRowIndex = Bot.GUI.DataGridConsole.RowCount - 1));
            Bot.GUI.DataGridConsole.Invoke(new Action(() => Bot.GUI.DataGridConsole.Refresh()));
        }

        public static void LoadGUISettings()
        {
            // Global
            //Bot.GUI.GlobalSettingsTab.SetSetting("AmountOfPokemonToDisplayOnStart", Bot._Session.LogicSettings.AmountOfPokemonToDisplayOnStart.ToString());
            //Bot.GUI.GlobalSettingsTab.SetSetting("AutoUpdate", Bot._Session.LogicSettings.AutoUpdate.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("TransferConfigAndAuthOnUpdate", Bot._Session.LogicSettings.TransferConfigAndAuthOnUpdate.ToString());
            //Bot.GUI.GlobalSettingsTab.SetSetting("ShowPokeballCountsBeforeRecycle", Bot._Session.LogicSettings.ShowPokeballCountsBeforeRecycle.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("DumpPokemonStats", Bot._Session.LogicSettings.DumpPokemonStats.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("TranslationLanguageCode", Bot._Session.LogicSettings.TranslationLanguageCode);
            Bot.GUI.GlobalSettingsTab.SetSetting("DisableHumanWalking", Bot._Session.LogicSettings.DisableHumanWalking.ToString());
            //Bot.GUI.GlobalSettingsTab.SetSetting("DefaultAltitude", Bot._Session.Settings.DefaultAltitude.ToString(".0"));
            Bot.GUI.GlobalSettingsTab.SetSetting("DefaultLatitude", Bot._Session.Settings.DefaultLatitude.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("DefaultLongitude", Bot._Session.Settings.DefaultLongitude.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("WalkingSpeedInKilometerPerHour", Bot._Session.LogicSettings.WalkingSpeedInKilometerPerHour.ToString(".0"));
            //Bot.GUI.GlobalSettingsTab.SetSetting("MaxSpawnLocationOffset", Bot._Session.LogicSettings.MaxSpawnLocationOffset.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("MaxTravelDistanceInMeters", Bot._Session.LogicSettings.MaxTravelDistanceInMeters.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("DelayBetweenPokemonCatch", Bot._Session.LogicSettings.DelayBetweenPokemonCatch.ToString());
            Bot.GUI.GlobalSettingsTab.SetSetting("DelayBetweenPlayerActions", Bot._Session.LogicSettings.DelayBetweenPlayerActions.ToString());
            //Bot.GUI.GlobalSettingsTab.SetSetting("GpxFile", Bot._Session.LogicSettings.GpxFile);
            //Bot.GUI.GlobalSettingsTab.SetSetting("UseGpxPathing", Bot._Session.LogicSettings.UseGpxPathing.ToString());

            // Sniping
            //Bot.GUI.SnipingSettingsTab.SetSetting("UseSnipeOnlineLocationServer", Bot._Session.LogicSettings.UseSnipeOnlineLocationServer.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("UseSnipeLocationServer", Bot._Session.LogicSettings.UseSnipeLocationServer.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("SnipeLocationServer", Bot._Session.LogicSettings.SnipeLocationServer.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("SnipeLocationServerPort", Bot._Session.LogicSettings.SnipeLocationServerPort.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("MinPokeballsToSnipe", Bot._Session.LogicSettings.MinPokeballsToSnipe.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("MinPokeballsWhileSnipe", Bot._Session.LogicSettings.MinPokeballsWhileSnipe.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("MinDelayBetweenSnipes", Bot._Session.LogicSettings.MinDelayBetweenSnipes.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("SnipingScanOffset", Bot._Session.LogicSettings.SnipingScanOffset.ToString("0.000"));
            Bot.GUI.SnipingSettingsTab.SetSetting("SnipeAtPokestops", Bot._Session.LogicSettings.SnipeAtPokestops.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("SnipeIgnoreUnknownIv", Bot._Session.LogicSettings.SnipeIgnoreUnknownIv.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("UseTransferIVForSnipe", Bot._Session.LogicSettings.UseTransferIvForSnipe.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("SnipePokemonNotInPokedex", Bot._Session.LogicSettings.SnipePokemonNotInPokedex.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("GetSniperInfoFromPokezz", Bot._Session.LogicSettings.GetSniperInfoFromPokezz.ToString());
            Bot.GUI.SnipingSettingsTab.SetSetting("GetOnlyVerifiedSniperInfoFromPokezz", Bot._Session.LogicSettings.GetOnlyVerifiedSniperInfoFromPokezz.ToString());
            Bot.GUI.SnipingSettingsTab.SetLocations(Bot._Session.LogicSettings.PokemonToSnipe.Locations.ToList());

            // Pokemons
            Bot.GUI.PokemonSettingsTab.SetSetting("AutomaticallyLevelUpPokemon", Bot._Session.LogicSettings.AutomaticallyLevelUpPokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("AmountOfTimesToUpgradeLoop", Bot._Session.LogicSettings.AmountOfTimesToUpgradeLoop.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("GetMinStarDustForLevelUp", Bot._Session.LogicSettings.GetMinStarDustForLevelUp.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("LevelUpByCPorIv", Bot._Session.LogicSettings.LevelUpByCPorIv != null ? Bot._Session.LogicSettings.LevelUpByCPorIv.ToString(): "or");
            Bot.GUI.PokemonSettingsTab.SetSetting("UpgradePokemonCpMinimum", Bot._Session.LogicSettings.UpgradePokemonCpMinimum.ToString("0.0"));
            Bot.GUI.PokemonSettingsTab.SetSetting("UpgradePokemonIvMinimum", Bot._Session.LogicSettings.UpgradePokemonIvMinimum.ToString("0.0"));
            Bot.GUI.PokemonSettingsTab.SetSetting("UpgradePokemonMinimumStatsOperator", Bot._Session.LogicSettings.UpgradePokemonMinimumStatsOperator.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("EvolveAboveIvValue", Bot._Session.LogicSettings.EvolveAboveIvValue.ToString("0.0"));
            Bot.GUI.PokemonSettingsTab.SetSetting("EvolveAllPokemonAboveIv", Bot._Session.LogicSettings.EvolveAllPokemonAboveIv.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("EvolveAllPokemonWithEnoughCandy", Bot._Session.LogicSettings.EvolveAllPokemonWithEnoughCandy.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("EvolveKeptPokemonsAtStorageUsagePercentage", Bot._Session.LogicSettings.EvolveKeptPokemonsAtStorageUsagePercentage.ToString("0.0"));
            Bot.GUI.PokemonSettingsTab.SetSetting("KeepPokemonsThatCanEvolve", Bot._Session.LogicSettings.KeepPokemonsThatCanEvolve.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("FavoriteMinIvPercentage", Bot._Session.LogicSettings.FavoriteMinIvPercentage.ToString("0.0"));
            Bot.GUI.PokemonSettingsTab.SetSetting("AutoFavoritePokemon", Bot._Session.LogicSettings.AutoFavoritePokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("RenamePokemon", Bot._Session.LogicSettings.RenamePokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("RenameAboveIv", Bot._Session.LogicSettings.RenameOnlyAboveIv.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("RenameTemplate", Bot._Session.LogicSettings.RenameTemplate.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("TransferWeakPokemon", Bot._Session.LogicSettings.TransferWeakPokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("TransferDuplicatePokemon", Bot._Session.LogicSettings.TransferDuplicatePokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("TransferDuplicatePokemonOnCapture", Bot._Session.LogicSettings.TransferDuplicatePokemonOnCapture.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("KeepMinCp", Bot._Session.LogicSettings.KeepMinCp.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("KeepMinIvPercentage", Bot._Session.LogicSettings.KeepMinIvPercentage.ToString("0.0"));
            Bot.GUI.PokemonSettingsTab.SetSetting("KeepMinOperator", Bot._Session.LogicSettings.KeepMinOperator.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("PrioritizeIvOverCp", Bot._Session.LogicSettings.PrioritizeIvOverCp.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("KeepMinDuplicatePokemon", Bot._Session.LogicSettings.KeepMinDuplicatePokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("UsePokemonToNotCatchFilter", Bot._Session.LogicSettings.UsePokemonToNotCatchFilter.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("UsePokemonSniperFilterOnly", Bot._Session.LogicSettings.UsePokemonSniperFilterOnly.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("KeepMinLvl", Bot._Session.LogicSettings.KeepMinLvl.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("UseKeepMinLvl", Bot._Session.LogicSettings.UseKeepMinLvl.ToString());
            Bot.GUI.PokemonSettingsTab.SetSetting("CatchPokemon", Bot._Session.LogicSettings.CatchPokemon.ToString());
            Bot.GUI.PokemonSettingsTab.SetPokemons(Bot._Session.LogicSettings);

            // Items
            Bot.GUI.ItemSettingsTab.SetSetting("VerboseRecycling", Bot._Session.LogicSettings.VerboseRecycling.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("RecycleInventoryAtUsagePercentage", Bot._Session.LogicSettings.RecycleInventoryAtUsagePercentage.ToString("0.0"));
            Bot.GUI.ItemSettingsTab.SetSetting("UseEggIncubators", Bot._Session.LogicSettings.UseEggIncubators.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseLuckyEggConstantly", Bot._Session.LogicSettings.UseLuckyEggConstantly.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseLuckyEggsMinPokemonAmount", Bot._Session.LogicSettings.UseLuckyEggsMinPokemonAmount.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseLuckyEggsWhileEvolving", Bot._Session.LogicSettings.UseLuckyEggsWhileEvolving.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseIncenseConstantly", Bot._Session.LogicSettings.UseIncenseConstantly.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseBerriesMinCp", Bot._Session.LogicSettings.UseBerriesMinCp.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseBerriesMinIv", Bot._Session.LogicSettings.UseBerriesMinIv.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseBerriesBelowCatchProbability", Bot._Session.LogicSettings.UseBerriesBelowCatchProbability.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseBerriesOperator", Bot._Session.LogicSettings.UseBerriesOperator.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseGreatBallAboveCp", Bot._Session.LogicSettings.UseGreatBallAboveCp.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseUltraBallAboveCp", Bot._Session.LogicSettings.UseUltraBallAboveCp.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseMasterBallAboveCp", Bot._Session.LogicSettings.UseMasterBallAboveCp.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseGreatBallAboveIv", Bot._Session.LogicSettings.UseGreatBallAboveIv.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseUltraBallAboveIv", Bot._Session.LogicSettings.UseUltraBallAboveIv.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("UseGreatBallBelowCatchProbability", Bot._Session.LogicSettings.UseGreatBallBelowCatchProbability.ToString("0.000"));
            Bot.GUI.ItemSettingsTab.SetSetting("UseUltraBallBelowCatchProbability", Bot._Session.LogicSettings.UseUltraBallBelowCatchProbability.ToString("0.000"));
            Bot.GUI.ItemSettingsTab.SetSetting("UseMasterBallBelowCatchProbability", Bot._Session.LogicSettings.UseMasterBallBelowCatchProbability.ToString("0.000"));
            Bot.GUI.ItemSettingsTab.SetSetting("MaxPokeballsPerPokemon", Bot._Session.LogicSettings.MaxPokeballsPerPokemon.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("TotalAmountOfPokeballsToKeep", Bot._Session.LogicSettings.TotalAmountOfPokeballsToKeep.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("TotalAmountOfPotionsToKeep", Bot._Session.LogicSettings.TotalAmountOfPotionsToKeep.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("TotalAmountOfRevivesToKeep", Bot._Session.LogicSettings.TotalAmountOfRevivesToKeep.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("TotalAmountOfBerriesToKeep", Bot._Session.LogicSettings.TotalAmountOfBerriesToKeep.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("EnableHumanizedThrows", Bot._Session.LogicSettings.EnableHumanizedThrows.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("NiceThrowChance", Bot._Session.LogicSettings.NiceThrowChance.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("GreatThrowChance", Bot._Session.LogicSettings.GreatThrowChance.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("ExcellentThrowChance", Bot._Session.LogicSettings.ExcellentThrowChance.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("CurveThrowChance", Bot._Session.LogicSettings.CurveThrowChance.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("ForceGreatThrowOverIv", Bot._Session.LogicSettings.ForceGreatThrowOverIv.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("ForceExcellentThrowOverIv", Bot._Session.LogicSettings.ForceExcellentThrowOverIv.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("ForceGreatThrowOverCp", Bot._Session.LogicSettings.ForceGreatThrowOverCp.ToString());
            Bot.GUI.ItemSettingsTab.SetSetting("ForceExcellentThrowOverCp", Bot._Session.LogicSettings.ForceExcellentThrowOverCp.ToString());

            Bot.GUI.ItemSettingsTab.SetItems(Bot._Session.LogicSettings.ItemRecycleFilter.ToList(), Bot.imagesList);
        }

        public static void SaveGUISettings()
        {
            try
            {
                // Global
                //Bot.GlobalSettings.AmountOfPokemonToDisplayOnStart = Convert.ToInt16(Bot.GUI.GlobalSettingsTab.GetSetting("AmountOfPokemonToDisplayOnStart"));
                //Bot.GlobalSettings.AutoUpdate = Convert.ToBoolean(Bot.GUI.GlobalSettingsTab.GetSetting("AutoUpdate"));
                Bot.GlobalSettings.TransferConfigAndAuthOnUpdate = Convert.ToBoolean(Bot.GUI.GlobalSettingsTab.GetSetting("TransferConfigAndAuthOnUpdate"));
                //Bot.GlobalSettings.ShowPokeballCountsBeforeRecycle = Convert.ToBoolean(Bot.GUI.GlobalSettingsTab.GetSetting("ShowPokeballCountsBeforeRecycle"));
                Bot.GlobalSettings.DumpPokemonStats = Convert.ToBoolean(Bot.GUI.GlobalSettingsTab.GetSetting("DumpPokemonStats"));
                Bot.GlobalSettings.TranslationLanguageCode = Bot.GUI.GlobalSettingsTab.GetSetting("TranslationLanguageCode");
                Bot.GlobalSettings.DisableHumanWalking = Convert.ToBoolean(Bot.GUI.GlobalSettingsTab.GetSetting("DisableHumanWalking"));
                //Bot.GlobalSettings.DefaultAltitude = Convert.ToDouble(Bot.GUI.GlobalSettingsTab.GetSetting("DefaultAltitude"));
                Bot.GlobalSettings.DefaultLatitude = Convert.ToDouble(Bot.GUI.GlobalSettingsTab.GetSetting("DefaultLatitude"));
                Bot.GlobalSettings.DefaultLongitude = Convert.ToDouble(Bot.GUI.GlobalSettingsTab.GetSetting("DefaultLongitude"));
                Bot.GlobalSettings.WalkingSpeedInKilometerPerHour = Convert.ToDouble(Bot.GUI.GlobalSettingsTab.GetSetting("WalkingSpeedInKilometerPerHour"));
                //Bot.GlobalSettings.MaxSpawnLocationOffset = Convert.ToInt32(Bot.GUI.GlobalSettingsTab.GetSetting("MaxSpawnLocationOffset"));
                Bot.GlobalSettings.MaxTravelDistanceInMeters = Convert.ToInt32(Bot.GUI.GlobalSettingsTab.GetSetting("MaxTravelDistanceInMeters"));
                Bot.GlobalSettings.DelayBetweenPokemonCatch = Convert.ToInt32(Bot.GUI.GlobalSettingsTab.GetSetting("DelayBetweenPokemonCatch"));
                Bot.GlobalSettings.DelayBetweenPlayerActions = Convert.ToInt32(Bot.GUI.GlobalSettingsTab.GetSetting("DelayBetweenPlayerActions"));
                //Bot.GlobalSettings.GpxFile = Bot.GUI.GlobalSettingsTab.GetSetting("GpxFile");
                //Bot.GlobalSettings.UseGpxPathing = Convert.ToBoolean(Bot.GUI.GlobalSettingsTab.GetSetting("UseGpxPathing"));

                // Sniping
                //Bot.GlobalSettings.UseSnipeOnlineLocationServer = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("UseSnipeOnlineLocationServer"));
                Bot.GlobalSettings.UseSnipeLocationServer = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("UseSnipeLocationServer"));
                Bot.GlobalSettings.SnipeLocationServer = Bot.GUI.SnipingSettingsTab.GetSetting("SnipeLocationServer");
                Bot.GlobalSettings.SnipeLocationServerPort = Convert.ToInt32(Bot.GUI.SnipingSettingsTab.GetSetting("SnipeLocationServerPort"));
                Bot.GlobalSettings.MinPokeballsToSnipe = Convert.ToInt32(Bot.GUI.SnipingSettingsTab.GetSetting("MinPokeballsToSnipe"));
                Bot.GlobalSettings.MinPokeballsWhileSnipe = Convert.ToInt32(Bot.GUI.SnipingSettingsTab.GetSetting("MinPokeballsWhileSnipe"));
                Bot.GlobalSettings.MinDelayBetweenSnipes = Convert.ToInt32(Bot.GUI.SnipingSettingsTab.GetSetting("MinDelayBetweenSnipes"));
                Bot.GlobalSettings.SnipingScanOffset = Convert.ToDouble(Bot.GUI.SnipingSettingsTab.GetSetting("SnipingScanOffset"));
                Bot.GlobalSettings.SnipeAtPokestops = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("SnipeAtPokestops"));
                Bot.GlobalSettings.SnipeIgnoreUnknownIv = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("SnipeIgnoreUnknownIv"));
                Bot.GlobalSettings.UseTransferIvForSnipe = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("UseTransferIVForSnipe"));
                Bot.GlobalSettings.SnipePokemonNotInPokedex = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("SnipePokemonNotInPokedex"));
                Bot.GlobalSettings.GetSniperInfoFromPokezz = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("GetSniperInfoFromPokezz"));
                Bot.GlobalSettings.GetOnlyVerifiedSniperInfoFromPokezz = Convert.ToBoolean(Bot.GUI.SnipingSettingsTab.GetSetting("GetOnlyVerifiedSniperInfoFromPokezz"));
                Bot.GlobalSettings.PokemonToSnipe.Locations = Bot.GUI.SnipingSettingsTab.GetLocations();

                // Pokemon
                Bot.GlobalSettings.AutomaticallyLevelUpPokemon = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("AutomaticallyLevelUpPokemon"));
                Bot.GlobalSettings.AmountOfTimesToUpgradeLoop = Convert.ToInt32(Bot.GUI.PokemonSettingsTab.GetSetting("AmountOfTimesToUpgradeLoop"));
                Bot.GlobalSettings.GetMinStarDustForLevelUp = Convert.ToInt32(Bot.GUI.PokemonSettingsTab.GetSetting("GetMinStarDustForLevelUp"));
                Bot.GlobalSettings.LevelUpByCPorIv = Bot.GUI.PokemonSettingsTab.GetSetting("LevelUpByCPorIv");
                Bot.GlobalSettings.UpgradePokemonCpMinimum = Convert.ToSingle(Bot.GUI.PokemonSettingsTab.GetSetting("UpgradePokemonCpMinimum"));
                Bot.GlobalSettings.UpgradePokemonIvMinimum = Convert.ToSingle(Bot.GUI.PokemonSettingsTab.GetSetting("UpgradePokemonIvMinimum"));
                Bot.GlobalSettings.UpgradePokemonMinimumStatsOperator = Bot.GUI.PokemonSettingsTab.GetSetting("UpgradePokemonMinimumStatsOperator");
                Bot.GlobalSettings.EvolveAboveIvValue = Convert.ToSingle(Bot.GUI.PokemonSettingsTab.GetSetting("EvolveAboveIvValue"));
                Bot.GlobalSettings.EvolveAllPokemonAboveIv = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("EvolveAllPokemonAboveIv"));
                Bot.GlobalSettings.EvolveAllPokemonWithEnoughCandy = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("EvolveAllPokemonWithEnoughCandy"));
                Bot.GlobalSettings.EvolveKeptPokemonsAtStorageUsagePercentage = Convert.ToDouble(Bot.GUI.PokemonSettingsTab.GetSetting("EvolveKeptPokemonsAtStorageUsagePercentage"));
                Bot.GlobalSettings.KeepPokemonsThatCanEvolve = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("KeepPokemonsThatCanEvolve"));
                Bot.GlobalSettings.FavoriteMinIvPercentage = Convert.ToSingle(Bot.GUI.PokemonSettingsTab.GetSetting("FavoriteMinIvPercentage"));
                Bot.GlobalSettings.AutoFavoritePokemon = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("AutoFavoritePokemon"));
                Bot.GlobalSettings.RenamePokemon = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("RenamePokemon"));
                Bot.GlobalSettings.RenameOnlyAboveIv = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("RenameAboveIv"));
                Bot.GlobalSettings.RenameTemplate = Bot.GUI.PokemonSettingsTab.GetSetting("RenameTemplate");
                Bot.GlobalSettings.TransferWeakPokemon = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("TransferWeakPokemon"));
                Bot.GlobalSettings.TransferDuplicatePokemon = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("TransferDuplicatePokemon"));
                Bot.GlobalSettings.TransferDuplicatePokemonOnCapture = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("TransferDuplicatePokemonOnCapture"));
                Bot.GlobalSettings.KeepMinCp = Convert.ToInt32(Bot.GUI.PokemonSettingsTab.GetSetting("KeepMinCp"));
                Bot.GlobalSettings.KeepMinIvPercentage = Convert.ToSingle(Bot.GUI.PokemonSettingsTab.GetSetting("KeepMinIvPercentage"));
                Bot.GlobalSettings.KeepMinOperator = Bot.GUI.PokemonSettingsTab.GetSetting("KeepMinOperator");
                Bot.GlobalSettings.PrioritizeIvOverCp = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("PrioritizeIvOverCp"));
                Bot.GlobalSettings.KeepMinDuplicatePokemon = Convert.ToInt32(Bot.GUI.PokemonSettingsTab.GetSetting("KeepMinDuplicatePokemon"));
                Bot.GlobalSettings.UsePokemonToNotCatchFilter = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("UsePokemonToNotCatchFilter"));
                Bot.GlobalSettings.UsePokemonSniperFilterOnly = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("UsePokemonSniperFilterOnly"));
                Bot.GlobalSettings.KeepMinLvl = Convert.ToInt32(Bot.GUI.PokemonSettingsTab.GetSetting("KeepMinLvl"));
                Bot.GlobalSettings.UseKeepMinLvl = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("UseKeepMinLvl"));
                Bot.GlobalSettings.CatchPokemon = Convert.ToBoolean(Bot.GUI.PokemonSettingsTab.GetSetting("CatchPokemon"));
                Bot.GlobalSettings.PokemonsNotToTransfer = Bot.GUI.PokemonSettingsTab.GetPokemonsNotToTransfer();
                Bot.GlobalSettings.PokemonsToEvolve = Bot.GUI.PokemonSettingsTab.GetPokemonsToEvolve();
                Bot.GlobalSettings.PokemonsToIgnore = Bot.GUI.PokemonSettingsTab.GetPokemonsToIgnore();
                Bot.GlobalSettings.PokemonsTransferFilter = Bot.GUI.PokemonSettingsTab.GetPokemonsTransferFilter();
                Bot.GlobalSettings.PokemonToSnipe.Pokemon = Bot.GUI.PokemonSettingsTab.GetPokemonsToSnipe();
                Bot.GlobalSettings.PokemonToUseMasterball = Bot.GUI.PokemonSettingsTab.GetPokemonsToUseMasterball();

                // Items
                Bot.GlobalSettings.VerboseRecycling = Convert.ToBoolean(Bot.GUI.ItemSettingsTab.GetSetting("VerboseRecycling"));
                Bot.GlobalSettings.RecycleInventoryAtUsagePercentage = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("RecycleInventoryAtUsagePercentage"));
                Bot.GlobalSettings.UseEggIncubators = Convert.ToBoolean(Bot.GUI.ItemSettingsTab.GetSetting("UseEggIncubators"));
                Bot.GlobalSettings.UseLuckyEggConstantly = Convert.ToBoolean(Bot.GUI.ItemSettingsTab.GetSetting("UseLuckyEggConstantly"));
                Bot.GlobalSettings.UseLuckyEggsMinPokemonAmount = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseLuckyEggsMinPokemonAmount"));
                Bot.GlobalSettings.UseLuckyEggsWhileEvolving = Convert.ToBoolean(Bot.GUI.ItemSettingsTab.GetSetting("UseLuckyEggsWhileEvolving"));
                Bot.GlobalSettings.UseIncenseConstantly = Convert.ToBoolean(Bot.GUI.ItemSettingsTab.GetSetting("UseIncenseConstantly"));
                Bot.GlobalSettings.UseBerriesMinCp = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseBerriesMinCp"));
                Bot.GlobalSettings.UseBerriesMinIv = Convert.ToSingle(Bot.GUI.ItemSettingsTab.GetSetting("UseBerriesMinIv"));
                Bot.GlobalSettings.UseBerriesBelowCatchProbability = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("UseBerriesBelowCatchProbability"));
                Bot.GlobalSettings.UseBerriesOperator = Bot.GUI.ItemSettingsTab.GetSetting("UseBerriesOperator");
                Bot.GlobalSettings.UseGreatBallAboveCp = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseGreatBallAboveCp"));
                Bot.GlobalSettings.UseUltraBallAboveCp = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseUltraBallAboveCp"));
                Bot.GlobalSettings.UseMasterBallAboveCp = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseMasterBallAboveCp"));
                Bot.GlobalSettings.UseGreatBallAboveIv = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseGreatBallAboveIv"));
                Bot.GlobalSettings.UseUltraBallAboveIv = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("UseUltraBallAboveIv"));
                Bot.GlobalSettings.UseGreatBallBelowCatchProbability = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("UseGreatBallBelowCatchProbability"));
                Bot.GlobalSettings.UseUltraBallBelowCatchProbability = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("UseUltraBallBelowCatchProbability"));
                Bot.GlobalSettings.UseMasterBallBelowCatchProbability = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("UseMasterBallBelowCatchProbability"));
                Bot.GlobalSettings.MaxPokeballsPerPokemon = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("MaxPokeballsPerPokemon"));
                Bot.GlobalSettings.TotalAmountOfPokeballsToKeep = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("TotalAmountOfPokeballsToKeep"));
                Bot.GlobalSettings.TotalAmountOfPotionsToKeep = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("TotalAmountOfPotionsToKeep"));
                Bot.GlobalSettings.TotalAmountOfRevivesToKeep = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("TotalAmountOfRevivesToKeep"));
                Bot.GlobalSettings.TotalAmountOfBerriesToKeep = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("TotalAmountOfBerriesToKeep"));
                Bot.GlobalSettings.ItemRecycleFilter = Bot.GUI.ItemSettingsTab.GetItems();
                Bot.GlobalSettings.EnableHumanizedThrows = Convert.ToBoolean(Bot.GUI.ItemSettingsTab.GetSetting("EnableHumanizedThrows"));
                Bot.GlobalSettings.NiceThrowChance = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("NiceThrowChance"));
                Bot.GlobalSettings.GreatThrowChance = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("GreatThrowChance"));
                Bot.GlobalSettings.ExcellentThrowChance = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("ExcellentThrowChance"));
                Bot.GlobalSettings.CurveThrowChance = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("CurveThrowChance"));
                Bot.GlobalSettings.ForceGreatThrowOverIv = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("ForceGreatThrowOverIv"));
                Bot.GlobalSettings.ForceExcellentThrowOverIv = Convert.ToDouble(Bot.GUI.ItemSettingsTab.GetSetting("ForceExcellentThrowOverIv"));
                Bot.GlobalSettings.ForceGreatThrowOverCp = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("ForceGreatThrowOverCp"));
                Bot.GlobalSettings.ForceExcellentThrowOverCp = Convert.ToInt32(Bot.GUI.ItemSettingsTab.GetSetting("ForceExcellentThrowOverCp"));

                Bot.GlobalSettings.Save(Bot.ProfileSettings.profilepath + "\\config\\config.json");

                if (Bot._Session.GUISettings.isStarted)
                {
                    MessageBox.Show("Profile has been saved, please restart bot to load new profile", "Profile saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Bot._Session = new Session(new ClientSettings(Bot.GlobalSettings), new LogicSettings(Bot.GlobalSettings));
                    Bot._Session.Client.ApiFailure = new ApiFailureStrategy(Bot._Session);
                    MessageBox.Show("Profile has been saved and has been loaded", "Profile saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Error while saving profile, check to make sure values are correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Navigation_UpdatePositionEvent(double lat, double lng)
        {
            SaveLocationToDisk(lat, lng);
        }

        private static void SaveLocationToDisk(double lat, double lng)
        {
            var coordsPath = Path.Combine(Directory.GetCurrentDirectory(), Bot.subPath, "Config", "LastPos.ini");

            File.WriteAllText(coordsPath, $"{lat}:{lng}");
        }
    }
}
