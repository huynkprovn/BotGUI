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
    class ManualEvolvePokemon
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
                    Message = "Waiting on last task to finish before we start evolving"
                });
                Bot._Session.GUISettings.isAwaitingPaused = true;
                while (Bot._Session.GUISettings.isAwaitingPaused == true)
                {
                    await Task.Delay(1000);
                }
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Starting to evolve pokemons"
                });
                await ManualEvolvePokemon.EvolvePokemonTask.AsyncStart(Bot._Session, pokemonsToTransfer, default(CancellationToken));
                foreach (var pokemon in pokemonsToTransfer)
                {
                    Bot.MyPokemons.Remove(pokemon.Key);
                }

                //UpdateMyPokemons();
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Done evolving"
                });
                Bot._Session.GUISettings.isPaused = false;
            }
        }

        public static class EvolvePokemonTask
        {
             public static Task AsyncStart(ISession session, Dictionary<ulong, PokemonData> pokemonList, CancellationToken cancellationToken)
            {
                return Task.Run(() => transfer(session, pokemonList, cancellationToken));
            }

            private static async Task transfer(ISession session, Dictionary<ulong, PokemonData> pokemonList, CancellationToken cancellationToken)
            {
                List<ulong> evolvedList = new List<ulong>();

                foreach (var pokemonToTransfer in pokemonList)
                {
                    var evolveResponse = await session.Client.Inventory.EvolvePokemon(pokemonToTransfer.Key);

                    session.EventDispatcher.Send(new PokemonEvolveEvent
                    {
                        Id = pokemonToTransfer.Value.PokemonId,
                        Exp = evolveResponse.ExperienceAwarded,
                        Result = evolveResponse.Result
                    });

                    await Task.Delay(1500, cancellationToken);

                    if (evolveResponse.Result == POGOProtos.Networking.Responses.EvolvePokemonResponse.Types.Result.Success)
                    {
                        evolvedList.Add(pokemonToTransfer.Key);
                    }
                }
                
            }

        }
    }
}
