using System;
using System.Globalization;
using System.Linq;
using POGOProtos.Networking.Responses;
using System.Collections.Generic;
using POGOProtos.Data.Player;
using PoGo.NecroBot.Logic;
using POGOProtos.Enums;
using Google.Protobuf.Collections;
using System.Windows.Forms;
using PoGo.NecroBot.GUI.Utils;

namespace PoGo.NecroBot.GUI.Util
{
    public delegate void GUIStatsDirtyDelegate();

    class GUIStats
    {
        private readonly DateTime _initSessionDateTime = DateTime.Now;
        public DateTime _lastUpdate = DateTime.Now;

        public void SetProfile(GetPlayerResponse profile) {
            Bot._ProfilePlayerName = profile.PlayerData.Username;
            Bot._ProfilePlayerTeam = profile.PlayerData.Team;
            Bot._ProfilePlayerMaxBagSpace = profile.PlayerData.MaxItemStorage;
            Bot._ProfilePlayerMaxPokemonSpace = profile.PlayerData.MaxPokemonStorage;
            Bot._ProfilePlayerStardust = profile.PlayerData.Currencies[1].Amount;
            Bot._ProfilePlayerPokecoins = profile.PlayerData.Currencies[0].Amount;

            UpdateProfile();
        }

        public void SetStats(Inventory inventory)
        {
            var stats = inventory.GetPlayerStats().Result;
            var stat = stats.FirstOrDefault();
            if (stat != null)
            {
                Bot._StatsPrevLevelXp = stat.PrevLevelXp;
                Bot._StatsNextLevelXp = stat.NextLevelXp;
                Bot._StatsExperience = stat.Experience;
                Bot._ProfilePlayerLevel = stat.Level;
            }
        }

        public void UpdateProfile()
        {
            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerName = Bot._ProfilePlayerName));
            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerLevel = Bot._ProfilePlayerLevel.ToString()));
            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerStardust = Bot._ProfilePlayerStardust.ToString()));
            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerPokecoins = Bot._ProfilePlayerPokecoins.ToString()));
        }

        public void UpdateStats()
        {
            int max = (int)Bot._StatsNextLevelXp - (int)Bot._StatsPrevLevelXp - GetXpDiff(Bot._ProfilePlayerLevel);
            int current = (int)Bot._StatsExperience - (int)Bot._StatsPrevLevelXp - GetXpDiff(Bot._ProfilePlayerLevel);

            if (current < max)
            {
                Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerLevelMaxExp = max));
                Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerLevelCurrentExp = current));
            }

            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerExperienceOverLevelExp = current.ToString() + "/" + max.ToString()));
            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerExperiencePerhour = Math.Round(Bot._StatsSessionExperience / GetRuntime(), 0).ToString() + " xp/hr"));
            Bot.GUI.Invoke(new Action(() => Bot.GUI.StatsPlayerPokemonPerHour = Math.Round(Bot._StatsSessionPokemon / GetRuntime(), 0).ToString() + " p/hr"));

            Bot.GUI.Invoke(new Action(() => Bot.GUI.Titlebar = this.ToString()));
        }

        public void Dirty(Inventory inventory)
        {
            SetStats(inventory);
            UpdateStats();
            DirtyEvent?.Invoke();
        }

        public event GUIStatsDirtyDelegate DirtyEvent;

        private string FormatRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).ToString(@"dd\.hh\:mm\:ss");
        }

        public double GetRuntime()
        {
            return (DateTime.Now - _initSessionDateTime).TotalSeconds / 3600;
        }

        public static int GetXpDiff(int level)
        {
            if (level > 0 && level <= 40)
            {
                int[] xpTable = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000,
                    10000, 10000, 10000, 10000, 15000, 20000, 20000, 20000, 25000, 25000,
                    50000, 75000, 100000, 125000, 150000, 190000, 200000, 250000, 300000, 350000,
                    500000, 500000, 750000, 1000000, 1250000, 1500000, 2000000, 2500000, 1000000, 1000000};
                return xpTable[level - 1];
            }
            return 0;
        }

        public override string ToString()
        {
            return
                $"{Bot._ProfilePlayerName} - Lvl: {Bot._ProfilePlayerLevel.ToString()} (Runtime: {FormatRuntime()})";
        }
    }
}
