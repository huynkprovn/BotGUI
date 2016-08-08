using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoGo.NecroBot.Logic;
using POGOProtos.Enums;
using PoGo.NecroBot.GUI.Utils;

namespace PoGo.NecroBot.GUI.UserControls
{
    public partial class PokemonSettingsControl : UserControl
    {
        public PokemonSettingsControl()
        {
            InitializeComponent();
        }

        public void SetSetting(string setting, string value)
        {
            Control[] control = Controls.Find(setting, true);
            if (control != null && control.Length > 0)
            {
                if (control[0] is TextBox)
                {
                    control[0].Text = value;
                }
                if (control[0] is CheckBox)
                {
                    ((CheckBox)control[0]).Checked = Convert.ToBoolean(value);
                }
                if(control[0] is ComboBox)
                {
                    control[0].Text = value;
                }

            }
        }

        public string GetSetting(string setting)
        {
            string value = "";

            Control[] control = Controls.Find(setting, true);
            if (control != null && control.Length > 0)
            {
                if (control[0] is TextBox)
                {
                    value = control[0].Text;
                }
                if (control[0] is CheckBox)
                {
                    value = Convert.ToString(((CheckBox)control[0]).Checked);
                }
                if (control[0] is ComboBox)
                {
                    value = control[0].Text;
                }

            }

            return value;
        }

        public void SetPokemons(ILogicSettings settings)
        {
            // Pokemon settings
            foreach (PokemonId pokemon in Enum.GetValues(typeof(PokemonId)))
            {
                // Skip pokemon = 0
                if ((int)pokemon > 0)
                {
                    // ToNotCatch
                    bool toNotCatch = false;
                    if (settings.PokemonsNotToCatch.Where(p => p == pokemon).ToList().Count > 0)
                        toNotCatch = true;

                    //ToEvolve
                    bool toEvolve = false;
                    if (settings.PokemonsToEvolve.Where(p => p == pokemon).ToList().Count > 0)
                        toEvolve = true;

                    // ToNotTransfer
                    bool toNotTransfer = false;
                    if (settings.PokemonsNotToTransfer.Where(p => p == pokemon).ToList().Count > 0)
                        toNotTransfer = true;

                    // ToSnipe
                    bool toSnipe = false;
                    if (settings.PokemonToSnipe.Pokemon.Where(p => p == pokemon).ToList().Count > 0)
                        toSnipe = true;

                    // ToUseMasterball
                    bool toUseMasterball = false;
                    if (settings.PokemonToUseMasterball != null && settings.PokemonToUseMasterball.Where(p => p == pokemon).ToList().Count > 0)
                        toUseMasterball = true;

                    // TransferFilters
                    int KeepMinCp = 0;
                    double KeepMinIvPercentage = 0.0;
                    int KeepMinDuplicatePokemon = 1;
                    string KeepMinOperator = "or";
                    bool PokemonsTransferFilter = false;
                    int KeepMinLvl = 0;
                    bool UseKeepMinLvl = false;

                    if (settings.PokemonsTransferFilter.ContainsKey(pokemon))
                    {
                        KeepMinCp = settings.PokemonsTransferFilter[pokemon].KeepMinCp;
                        KeepMinLvl = settings.PokemonsTransferFilter[pokemon].KeepMinLvl;
                        UseKeepMinLvl = settings.PokemonsTransferFilter[pokemon].UseKeepMinLvl;
                        KeepMinIvPercentage = settings.PokemonsTransferFilter[pokemon].KeepMinIvPercentage;
                        KeepMinDuplicatePokemon = settings.PokemonsTransferFilter[pokemon].KeepMinDuplicatePokemon;
                        KeepMinOperator = settings.PokemonsTransferFilter[pokemon].KeepMinOperator;
                        PokemonsTransferFilter = true;
                    }

                    Bitmap bmp = new Bitmap(40, 30);
                    Bot.imagesList.TryGetValue("pokemon_" + ((int)pokemon).ToString(), out bmp);
                    dataPokemon.Invoke(new Action(() => dataPokemon.Rows.Add((int)pokemon, bmp, pokemon.ToString(), (int) pokemon, KeepMinCp, KeepMinLvl, UseKeepMinLvl, KeepMinOperator, KeepMinIvPercentage, PokemonsTransferFilter, KeepMinDuplicatePokemon, toNotTransfer, toEvolve, toNotCatch, toSnipe, toUseMasterball)));
                }
            }
        }

        //List<PokemonId> PokemonsNotToTransfer = new List<PokemonId>();
        //List<PokemonId> PokemonsToEvolve = new List<PokemonId>();
        //List<PokemonId> PokemonsToIgnore = new List<PokemonId>();
        //Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter = new Dictionary<PokemonId, TransferFilter>();
        //List<PokemonId> PokemonsToSnipe = new List<PokemonId>();

        public List<PokemonId> GetPokemonsNotToTransfer()
        {
            List<PokemonId> PokemonsNotToTransfer = new List<PokemonId>();
 
            foreach (DataGridViewRow row in dataPokemon.Rows)
            {
                // 6 = PokemonsNotToTransfer
                if ((bool)row.Cells["dataPokemonColNotToTransfer"].Value == true)
                    PokemonsNotToTransfer.Add((PokemonId)row.Cells["dataPokemonColID"].Value);
            }

            return PokemonsNotToTransfer;
        }

        public List<PokemonId> GetPokemonsToEvolve()
        {
            List<PokemonId> PokemonsToEvolve = new List<PokemonId>();

            foreach (DataGridViewRow row in dataPokemon.Rows)
            {
                // 7 = PokemonsToEvolve
                if ((bool)row.Cells["dataPokemonColToEvolve"].Value == true)
                    PokemonsToEvolve.Add((PokemonId)row.Cells["dataPokemonColID"].Value);
            }

            return PokemonsToEvolve;
        }

        public List<PokemonId> GetPokemonsToIgnore()
        {
            List<PokemonId> PokemonsToIgnore = new List<PokemonId>();

            foreach (DataGridViewRow row in dataPokemon.Rows)
            {
                // 8 = PokemonsToIgnore
                if ((bool)row.Cells["dataPokemonColToIgnore"].Value == true)
                    PokemonsToIgnore.Add((PokemonId)row.Cells["dataPokemonColID"].Value);
            }

            return PokemonsToIgnore;
        }

        public Dictionary<PokemonId, TransferFilter> GetPokemonsTransferFilter()
        {
            Dictionary<PokemonId, TransferFilter> PokemonsTransferFilter = new Dictionary<PokemonId, TransferFilter>();

            foreach (DataGridViewRow row in dataPokemon.Rows)
            {
                if(Convert.ToBoolean(row.Cells["dataPokemonColPokemonsTransferFilter"].Value))
                {
                    // 3 = KeepMinCp, 4 = KeepMinIV, 5 = KeepMinDuplicate
                    TransferFilter newFilter = new TransferFilter();
                    newFilter.KeepMinCp = Convert.ToInt16(row.Cells["dataPokemonColKeepMinCp"].Value);
                    newFilter.KeepMinLvl = Convert.ToInt16(row.Cells["dataPokemonColKeepMinLvl"].Value);
                    newFilter.UseKeepMinLvl = Convert.ToBoolean(row.Cells["dataPokemonColUseKeepMinLvl"].Value);
                    newFilter.KeepMinIvPercentage = Convert.ToSingle(row.Cells["dataPokemonColKeepMinIv"].Value);
                    newFilter.KeepMinOperator = row.Cells["dataPokemonColKeepMinOperator"].Value.ToString();
                    newFilter.KeepMinDuplicatePokemon = Convert.ToInt16(row.Cells["dataPokemonColKeepMinDuplicate"].Value);
                    PokemonsTransferFilter.Add((PokemonId)row.Cells["dataPokemonColID"].Value, newFilter);

                }
            }

            return PokemonsTransferFilter;
        }
        public List<PokemonId> GetPokemonsToSnipe()
        {
            List<PokemonId> PokemonsToSnipe = new List<PokemonId>();

            foreach (DataGridViewRow row in dataPokemon.Rows)
            {
                // 9 = PokemonToSnipe
                if ((bool)row.Cells["dataPokemonColToSnipe"].Value == true)
                    PokemonsToSnipe.Add((PokemonId)row.Cells["dataPokemonColID"].Value);
            }

            return PokemonsToSnipe;
        }

        public List<PokemonId> GetPokemonsToUseMasterball()
        {
            List<PokemonId> PokemonsToUseMasterball = new List<PokemonId>();

            foreach (DataGridViewRow row in dataPokemon.Rows)
            {
                // 10 = ToUseMasterball
                if ((bool)row.Cells["dataPokemonColToUseMasterball"].Value == true)
                    PokemonsToUseMasterball.Add((PokemonId)row.Cells["dataPokemonColID"].Value);
            }

            return PokemonsToUseMasterball;
        }
    }
}
