using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Enums;
using POGOProtos.Networking.Responses;
using System.Net.Sockets;
using System.Threading;
using PoGo.NecroBot.Logic.Tasks;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.PoGoUtils;
using POGOProtos.Data;
using System.Windows.Forms;
using PoGo.NecroBot.GUI.Utils;

namespace PoGo.NecroBot.GUI.Tasks
{
    class ManualTransferPokemon
    {
        public async static void Execute()
        {
            Dictionary<ulong, PokemonData> pokemonsToTransfer = new Dictionary<ulong, PokemonData>();

            foreach (DataGridViewRow row in Bot.GUI.DataGridMyPokemons.Rows)
            {
                if (row.Selected == true)
                {
                    pokemonsToTransfer.Add((ulong)row.Cells[0].Value, Bot.MyPokemons[(ulong)row.Cells[0].Value]);
                }
            }

            if (pokemonsToTransfer.Count > 0 && Bot._Session.GUISettings.isStarted)
            {
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Waiting on last task to finish before we start transfering"
                });
                Bot._Session.GUISettings.isAwaitingPaused = true;
                while (Bot._Session.GUISettings.isAwaitingPaused == true)
                {
                    await Task.Delay(1000);
                }
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Starting to transfer pokemons"
                });
                await ManualTransferPokemon.TransferPokemonTask.AsyncStart(Bot._Session, pokemonsToTransfer, default(CancellationToken));
                foreach (var pokemon in pokemonsToTransfer)
                {
                    Bot.MyPokemons.Remove(pokemon.Key);
                }

                //UpdateMyPokemons();
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Done transfering"
                });
                Bot._Session.GUISettings.isPaused = false;
            }
        }

        public static class TransferPokemonTask
        {
            public static Task AsyncStart(ISession session, Dictionary<ulong, PokemonData> pokemonList, CancellationToken cancellationToken)
            {
                return Task.Run(() => transfer(session, pokemonList, cancellationToken));
            }

            private static async Task transfer(ISession session, Dictionary<ulong, PokemonData> pokemonList, CancellationToken cancellationToken)
            {
                foreach (var pokemonToTransfer in pokemonList)
                {
                    await session.Client.Inventory.TransferPokemon(pokemonToTransfer.Key);
                    await session.Inventory.DeletePokemonFromInvById(pokemonToTransfer.Key);

                    var pokemonSettings = await session.Inventory.GetPokemonSettings();
                    var pokemonFamilies = await session.Inventory.GetPokemonFamilies();

                    var bestPokemonOfType = (session.LogicSettings.PrioritizeIvOverCp
                        ? await session.Inventory.GetHighestPokemonOfTypeByIv(pokemonToTransfer.Value)
                        : await session.Inventory.GetHighestPokemonOfTypeByCp(pokemonToTransfer.Value)) ?? pokemonToTransfer.Value;

                    var setting = pokemonSettings.Single(q => q.PokemonId == pokemonToTransfer.Value.PokemonId);
                    var family = pokemonFamilies.First(q => q.FamilyId == setting.FamilyId);

                    family.Candy_++;

                    session.EventDispatcher.Send(new TransferPokemonEvent
                    {
                        Id = pokemonToTransfer.Value.PokemonId,
                        Perfection = PokemonInfo.CalculatePokemonPerfection(pokemonToTransfer.Value),
                        Cp = pokemonToTransfer.Value.Cp,
                        BestCp = bestPokemonOfType.Cp,
                        BestPerfection = PokemonInfo.CalculatePokemonPerfection(bestPokemonOfType),
                        FamilyCandies = family.Candy_
                    });

                    await Task.Delay(1000, cancellationToken);
                }

            }

        }
    }
}
