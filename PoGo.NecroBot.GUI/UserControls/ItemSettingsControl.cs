using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POGOProtos.Inventory.Item;

namespace PoGo.NecroBot.GUI.UserControls
{
    public partial class ItemSettingsControl : UserControl
    {
        public ItemSettingsControl()
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
                if (control[0] is ComboBox)
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

        public void SetItems(List<KeyValuePair<ItemId, int>> items, Dictionary<string, Bitmap> imagesList)
        {
            // Item settings
            foreach (ItemId item in Enum.GetValues(typeof(ItemId)))
            {
                int KeepMax = (int)items.Where(i => i.Key == item).FirstOrDefault().Value;

                Bitmap bmp = new Bitmap(40, 30);
                imagesList.TryGetValue("item_" + ((int)item).ToString(), out bmp);
                dataItems.Rows.Add((int)item, bmp, item, KeepMax);
            }
        }

        public List<KeyValuePair<ItemId, int>> GetItems()
        {
            List<KeyValuePair<ItemId, int>> itemsList = new List<KeyValuePair<ItemId, int>>();

            foreach (DataGridViewRow row in dataItems.Rows)
            {
                try
                {
                    itemsList.Add(new KeyValuePair<ItemId, int>((ItemId)row.Cells[2].Value, Convert.ToInt16(row.Cells[3].Value)));
                }
                catch { }
            }

            return itemsList;
        }
    }
}
