using System;
using System.Globalization;
using System.Linq;
using POGOProtos.Networking.Responses;
using System.Collections.Generic;
using POGOProtos.Data.Player;
using PoGo.NecroBot.Logic;
using POGOProtos.Enums;
using Google.Protobuf.Collections;
using POGOProtos.Inventory.Item;
using POGOProtos.Inventory;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Settings.Master;
using PoGo.NecroBot.GUI.Utils;
using System.Windows.Forms;
using System.Drawing;

namespace PoGo.NecroBot.GUI.Util
{
    public delegate void GUIItemsDirtyDelegate();

    class GUIItems
    {
        public async void SetItems(Inventory inventory)
        {
            if(Bot.PokemonSettings.Count == 0)
            {
                var settings = await Bot._Session.Inventory.GetPokemonSettings();
                Bot.PokemonSettings = settings.ToList(); 
            }

            var items = inventory.GetItems().Result;
            if(items != null)
            {
                foreach (var item in items.ToList())
                {
                    if (Bot.MyItems.ContainsKey(item.ItemId))
                        Bot.MyItems[item.ItemId] = item.Count;
                    else
                        Bot.MyItems.Add(item.ItemId, item.Count);
                }
            }
            var candies = inventory.GetPokemonFamilies().Result;
            if(candies != null)
            {
                foreach (var candy in candies.ToList())
                {
                    if (Bot.MyCandies.ContainsKey(candy.FamilyId))
                        Bot.MyCandies[candy.FamilyId] = candy.Candy_;
                    else
                        Bot.MyCandies.Add(candy.FamilyId, candy.Candy_);
                }
            }
        }

        public void UpdateItemByValue(ItemId itemId, int value)
        {
            if (Bot.MyItems.ContainsKey(itemId))
                Bot.MyItems[itemId] += value;
            else
                Bot.MyItems.Add(itemId, 1);

        }

        public void UpdateItemByCount(ItemId itemId, int count)
        {
            if (Bot.MyItems.ContainsKey(itemId))
                Bot.MyItems[itemId] = count;
            else
                Bot.MyItems.Add(itemId, count);
        }

        public void UpdateCandyValue(PokemonId pokemonid, int value, Session session)
        {
            var setting = Bot.PokemonSettings.Single(q => q.PokemonId == pokemonid);

            if (Bot.MyCandies.ContainsKey(setting.FamilyId))
                Bot.MyCandies[setting.FamilyId] = value;
            else
                Bot.MyCandies.Add(setting.FamilyId, value);
        }

        public void UpdateCandyByValue(PokemonId pokemonid, int value, Session session)
        {
            var setting = Bot.PokemonSettings.Single(q => q.PokemonId == pokemonid);

            if (Bot.MyCandies.ContainsKey(setting.FamilyId))
                Bot.MyCandies[setting.FamilyId] += value;
            else
                Bot.MyCandies.Add(setting.FamilyId, value);
        }

        public void UpdateItemByItemsString(string items)
        {
            string[] itemsList = Array.ConvertAll(items.Split(','), i => i.Trim());
            foreach(var item in itemsList)
            {
                ItemId itemId = (ItemId)Enum.Parse(typeof(ItemId), ((item.Substring(item.LastIndexOf(' ')))));
                int count = Convert.ToInt16(item.Substring(0, item.IndexOf(' ')));
                if (Bot.MyItems.ContainsKey(itemId))
                    Bot.MyItems[itemId] += count;
                else
                    Bot.MyItems.Add(itemId, count);
            }

        }

        private void UpdateMyItemsCandies()
        {
            // Items
            int total = 0;

            var currentItemList = Bot.GUI.DataGridMyItems.Rows.OfType<DataGridViewRow>().ToArray();

            foreach (var line in currentItemList)
            {
                if (Bot.MyItems.Where(i => i.Key == (ItemId)line.Cells[0].Value).ToList().Count == 0)
                {
                    Bot.GUI.DataGridMyItems.Invoke(new Action(() => Bot.GUI.DataGridMyItems.Rows.Remove(line)));
                }
            }

            foreach (var item in Bot.MyItems)
            {
                if (currentItemList.Where(p => (ItemId)p.Cells[0].Value == item.Key).Count() == 0)
                {
                    object name = Enum.Parse(typeof(ItemId), ((int)item.Key).ToString());
                    Bitmap bmp = new Bitmap(40, 30);
                    if (Bot.imagesList.TryGetValue("item_" + ((int)item.Key).ToString(), out bmp))
                        Bot.GUI.DataGridMyItems.Invoke(new Action(() => Bot.GUI.DataGridMyItems.Rows.Add(item.Key, bmp, name, item.Value)));
                    else
                        Bot.GUI.DataGridMyItems.Invoke(new Action(() => Bot.GUI.DataGridMyItems.Rows.Add(item.Key, new Bitmap(40, 30), name, item.Value)));
                }
                else
                {
                    DataGridViewRow row = currentItemList.Where(p => (ItemId)p.Cells[0].Value == item.Key).FirstOrDefault();
                    if (row != null)
                        Bot.GUI.DataGridMyItems.Invoke(new Action(() => Bot.GUI.DataGridMyItems[3, row.Index].Value = item.Value < 0 ? 0:item.Value));
                }
            }

            for (int i = 0; i < Bot.GUI.DataGridMyItems.Rows.Count; ++i)
            {
                total += Convert.ToInt32(Bot.GUI.DataGridMyItems.Rows[i].Cells[3].Value);
            }

            Bot.GUI.Invoke(new Action(() => Bot.GUI.DataGridMyItemsTab = "Items (" + total.ToString() + "/" + Bot._ProfilePlayerMaxBagSpace.ToString()));

            // Candies
            var currentCandyList = Bot.GUI.DataGridMyCandies.Rows.OfType<DataGridViewRow>().ToArray();

            foreach (var line in currentCandyList)
            {
                if (Bot.MyCandies.Where(i => i.Key == (PokemonFamilyId)line.Cells[0].Value).ToList().Count == 0)
                {
                    Bot.GUI.DataGridMyCandies.Invoke(new Action(() => Bot.GUI.DataGridMyCandies.Rows.Remove(line)));
                }
            }

            foreach (var candy in Bot.MyCandies)
            {
                if (currentCandyList.Where(p => (PokemonFamilyId)p.Cells[0].Value == candy.Key).Count() == 0)
                {
                    Bot.GUI.DataGridMyCandies.Invoke(new Action(() => Bot.GUI.DataGridMyCandies.Rows.Add(candy.Key, candy.Key.ToString(), candy.Value)));
                }
                else
                {
                    DataGridViewRow row = currentCandyList.Where(p => (PokemonFamilyId)p.Cells[0].Value == candy.Key).FirstOrDefault();
                    if (row != null)
                        Bot.GUI.DataGridMyCandies.Invoke(new Action(() => Bot.GUI.DataGridMyCandies[2, row.Index].Value = candy.Value));
                }
            }
        }

        public void Dirty(Inventory inventory)
        {
            UpdateMyItemsCandies();
            DirtyEvent?.Invoke();
        }

        public event GUIItemsDirtyDelegate DirtyEvent;
    }
}
