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
using GMap.NET;
using System.Text.RegularExpressions;
using PoGo.NecroBot.GUI.Utils;
using System.Globalization;
using PoGo.NecroBot.Logic.PoGoUtils;
using POGOProtos.Map.Pokemon;
using PoGo.NecroBot.Logic.Common;
using POGOProtos.Data;

namespace PoGo.NecroBot.GUI.Tasks
{
    class ManualSnipePokemon
    {
        public async static void Execute(SniperInfo snipeFromFeed)
        {
            Bot._Session.GUISettings.isSniping = true;
            Dictionary<PokemonId, PointLatLng> snipeList = new Dictionary<PokemonId, PointLatLng>();
            snipeList.Add((PokemonId)Enum.Parse(typeof(PokemonId), snipeFromFeed.Id.ToString()), new PointLatLng(snipeFromFeed.Latitude, snipeFromFeed.Longitude));

            if (snipeList.Count > 0 && Bot._Session.GUISettings.isStarted)
            {
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Waiting on last task to finish before we start sniping"
                });
                Bot._Session.GUISettings.isAwaitingPaused = true;
                while (Bot._Session.GUISettings.isAwaitingPaused == true)
                {
                    await Task.Delay(1000);
                }
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Starting to snipe list"
                });
                await ManualSnipePokemon.SnipePokemonTask.AsyncStart(Bot._Session, snipeList, default(CancellationToken));
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Done sniping"
                });
                Bot._Session.GUISettings.isPaused = false;
            }

            Bot._Session.GUISettings.isSniping = false;
        }

        public async static void Execute()
        {
            Bot._Session.GUISettings.isSniping = true;
            Dictionary<PokemonId, PointLatLng> snipeList = new Dictionary<PokemonId, PointLatLng>();
            foreach (var line in Bot.GUI.SnipingTextBox.Lines)
            {
                try
                {
                    string parsed = ParseSnipeMessage(line);

                    List<string> splitString = parsed.Split(' ').Select(s => s.Trim()).ToList();
                    double lat = Convert.ToDouble(splitString[1]);
                    double lng = Convert.ToDouble(splitString[2]);
                    string name = splitString[0];
                    int IV = Convert.ToInt16(splitString[3]);

  
                    if (Bot.GUI.SnipeOptionAll)
                    {
                        snipeList.Add((PokemonId)Enum.Parse(typeof(PokemonId), name), new PointLatLng(lat, lng));
                    }
                    else
                    {
                        if (Bot.GlobalSettings.PokemonToSnipe.Pokemon.Where(p => p == (PokemonId)Enum.Parse(typeof(PokemonId), name)).ToList().Count > 0 && IV >= Bot.GlobalSettings.KeepMinIvPercentage)
                        {
                            snipeList.Add((PokemonId)Enum.Parse(typeof(PokemonId), name), new PointLatLng(lat, lng));
                        }
                    }
                }
                catch
                {
                    //UpdateMyPokemons();
                    Bot._Session.EventDispatcher.Send(new ErrorEvent
                    {
                        Message = "Bad sniping syntax: " + line
                    });
                    continue;
                }
            }

            if (snipeList.Count > 0 && Bot._Session.GUISettings.isStarted)
            {
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Waiting on last task to finish before we start sniping"
                });
                Bot._Session.GUISettings.isAwaitingPaused = true;
                while (Bot._Session.GUISettings.isAwaitingPaused == true)
                {
                    await Task.Delay(1000);
                }
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Starting to snipe list"
                });
                await ManualSnipePokemon.SnipePokemonTask.AsyncStart(Bot._Session, snipeList, default(CancellationToken));
                Bot._Session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Done sniping"
                });
                Bot._Session.GUISettings.isPaused = false;
            }

            Bot._Session.GUISettings.isSniping = false;

            Bot.GUI.SnipingTextBox.Invoke(new Action(() => Bot.GUI.SnipingTextBox.Text = ""));
        }

        public static string ParseSnipeMessage(string message)
        {
            Dictionary<string, PokemonId> pokemonList = new Dictionary<string, PokemonId>();

            foreach (PokemonId pokemon in Enum.GetValues(typeof(PokemonId)))
                pokemonList.Add(pokemon.ToString().ToLower(), pokemon);

            message = message.ToLower().Replace('%', ' ').Replace("iv", " ").Replace(',', ' ').Replace(". ", " ").Replace(':', ' ');
            if (message.Contains("[") && message.Contains("]"))
                message = Regex.Match(message, @"\]([^)]*)\[").Groups[1].Value;

            List<string> splitString = message.Split(' ').Select(s => s.Trim()).ToList();

            double lat = 0;
            double lng = 0;
            PokemonId pokeId = PokemonId.Missingno;
            int IV = 0;

            double doubleTemp;
            int intTemp;
            int doubleCount = 0;

            foreach (var info in splitString)
            {
                if (Int32.TryParse(info, out intTemp))
                {
                    IV = intTemp;
                }
                else {
                    if (Double.TryParse(info, out doubleTemp))
                    {
                        doubleCount++;
                        if (doubleCount == 1)
                            lat = doubleTemp;

                        if (doubleCount == 2)
                            lng = doubleTemp;

                        if (doubleCount == 3 && IV == 0)
                            IV = Convert.ToInt16(doubleTemp);
                    }
                    else
                    {
                        if (pokemonList.ContainsKey(info.ToLower()))
                            pokeId = pokemonList[info.ToLower()];
                    }
                }
            }

            return pokeId.ToString() + " " + lat.ToString() + " " +  lng.ToString() + " " + IV.ToString();
        }

        public static class SnipePokemonTask
        {
            public static Task AsyncStart(ISession session, Dictionary<PokemonId, PointLatLng> pokemonIdList, CancellationToken cancellationToken)
            {
                return Task.Run(() => snipe(session, pokemonIdList, cancellationToken));
            }

            private static async Task snipe(ISession session, Dictionary<PokemonId, PointLatLng> pokemonIdList, CancellationToken cancellationToken)
            {
                var currentLatitude = session.Client.CurrentLatitude;
                var currentLongitude = session.Client.CurrentLongitude;

                int snipingCount = 0;


                foreach(var pokemonToSnipe in pokemonIdList)
                {
                    List<MapPokemon> catchablePokemon;
                    try
                    {
                        await
                            session.Client.Player.UpdatePlayerLocation(pokemonToSnipe.Value.Lat, pokemonToSnipe.Value.Lng, session.Client.CurrentAltitude);

                        session.EventDispatcher.Send(new UpdatePositionEvent
                        {
                            Longitude = pokemonToSnipe.Value.Lng,
                            Latitude = pokemonToSnipe.Value.Lat
                        });

                        var mapObjects = session.Client.Map.GetMapObjects().Result;
                        catchablePokemon =
                            mapObjects.Item1.MapCells.SelectMany(q => q.CatchablePokemons)
                                .Where(q => pokemonToSnipe.Key == q.PokemonId)
                                .OrderByDescending(pokemon => PokemonInfo.CalculateMaxCpMultiplier(pokemonToSnipe.Key))
                                .ToList();
                    }
                    finally
                    {
                        await
                            session.Client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude, session.Client.CurrentAltitude);
                    }

                    foreach (var pokemon in catchablePokemon)
                    {
                        EncounterResponse encounter;
                        try
                        {
                            await
                                session.Client.Player.UpdatePlayerLocation(pokemonToSnipe.Value.Lat, pokemonToSnipe.Value.Lng, session.Client.CurrentAltitude);

                            encounter =
                                session.Client.Encounter.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnPointId).Result;
                        }
                        finally
                        {
                            await
                                session.Client.Player.UpdatePlayerLocation(currentLatitude, currentLongitude,
                                    session.Client.CurrentAltitude);
                        }

                        if (encounter.Status == EncounterResponse.Types.Status.EncounterSuccess)
                        {
                            session.EventDispatcher.Send(new UpdatePositionEvent
                            {
                                Latitude = currentLatitude,
                                Longitude = currentLongitude
                            });

                            await CatchPokemonTask.Execute(session, cancellationToken, encounter, pokemon);
                        }
                        else if (encounter.Status == EncounterResponse.Types.Status.PokemonInventoryFull)
                        {
                            if (session.LogicSettings.EvolveAllPokemonAboveIv ||
                                session.LogicSettings.EvolveAllPokemonWithEnoughCandy)
                            {
                                await EvolvePokemonTask.Execute(session, cancellationToken);
                            }

                            if (session.LogicSettings.TransferDuplicatePokemon)
                            {
                                await TransferDuplicatePokemonTask.Execute(session, cancellationToken);
                            }
                            else
                            {
                                session.EventDispatcher.Send(new WarnEvent
                                {
                                    Message = session.Translation.GetTranslation(TranslationString.InvFullTransferManually)
                                });
                            }
                        }
                        else
                        {
                            session.EventDispatcher.Send(new WarnEvent
                            {
                                Message =
                                    session.Translation.GetTranslation(
                                        TranslationString.EncounterProblem, encounter.Status)
                            });
                        }

                        if (
                            !Equals(catchablePokemon.ElementAtOrDefault(catchablePokemon.Count - 1),
                                pokemon))
                        {
                            await Task.Delay(session.LogicSettings.DelayBetweenPokemonCatch, cancellationToken);
                        }
                    }

                    session.EventDispatcher.Send(new SnipeModeEvent { Active = false });
                    await Task.Delay(session.LogicSettings.DelayBetweenPlayerActions, cancellationToken);
                }
            }
        }
    }
}
