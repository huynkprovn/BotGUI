using PoGo.NecroBot.GUI.Utils;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoGo.NecroBot.GUI.Tasks
{
    class ManualPowerUpPokemon
    {
        public async static void Execute()
        {
            Dictionary<ulong, PokemonData> pokemonsToPowerUp = new Dictionary<ulong, PokemonData>();

            foreach (DataGridViewRow row in Bot.GUI.DataGridMyPokemons.Rows)
            {
                if (row.Selected == true)
                {
                    pokemonsToPowerUp.Add((ulong)row.Cells[0].Value, Bot.MyPokemons[(ulong)row.Cells[0].Value]);
                }
            }

            if (pokemonsToPowerUp.Count > 0 && Bot._Session.GUISettings.isStarted)
            {
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Waiting on last task to finish before we start powering up"
                });
                Bot._Session.GUISettings.isAwaitingPaused = true;
                while (Bot._Session.GUISettings.isAwaitingPaused == true)
                {
                    await Task.Delay(1000);
                }
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Starting to powerup pokemons"
                });
                await ManualTransferPokemon.TransferPokemonTask.AsyncStart(Bot._Session, pokemonsToPowerUp, default(CancellationToken));

                //UpdateMyPokemons();
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Done powering up"
                });
                Bot._Session.GUISettings.isPaused = false;
            }
        }

        public static class PowerUpPokemonTask
        {
            public static Task AsyncStart(ISession session, Dictionary<ulong, PokemonData> pokemonList, CancellationToken cancellationToken)
            {
                return Task.Run(() => powerup(session, pokemonList, cancellationToken));
            }

            private static async Task powerup(ISession session, Dictionary<ulong, PokemonData> pokemonList, CancellationToken cancellationToken)
            {
                if (await session.Inventory.GetStarDust() <= session.LogicSettings.GetMinStarDustForLevelUp)
                    return;

                var myPokemonSettings = await session.Inventory.GetPokemonSettings();
                var pokemonSettings = myPokemonSettings.ToList();

                var myPokemonFamilies = await session.Inventory.GetPokemonFamilies();
                var pokemonFamilies = myPokemonFamilies.ToArray();

                var upgradedNumber = 0;
  
                foreach (var pokemonToPowerup in pokemonList)
                {
                    if (PokemonInfo.CalculateMaxCp(pokemonToPowerup.Value) == pokemonToPowerup.Value.Cp) continue;

                    var settings = pokemonSettings.Single(x => x.PokemonId == pokemonToPowerup.Value.PokemonId);
                    var familyCandy = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId);

                    if (familyCandy.Candy_ <= 0) continue;

                    var upgradeResult = await session.Inventory.UpgradePokemon(pokemonToPowerup.Value.Id);
                    if (upgradeResult.Result.ToString().ToLower().Contains("success"))
                    {
                        Logger.Write("Pokemon Upgraded:" + session.Translation.GetPokemonTranslation(upgradeResult.UpgradedPokemon.PokemonId) + ":" +
                                        upgradeResult.UpgradedPokemon.Cp);
                        upgradedNumber++;
                    }

                    if (upgradedNumber >= session.LogicSettings.AmountOfTimesToUpgradeLoop)
                        break;
                }
   
            }
        }
            
    }
}
