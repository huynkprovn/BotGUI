using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GMap.NET;
using POGOProtos.Map.Pokemon;
using PoGo.NecroBot.Logic.PoGoUtils;
using POGOProtos.Networking.Responses;
using PoGo.NecroBot.Logic.Common;

namespace PoGo.NecroBot.Logic.Tasks
{
    class AutoSnipePokemonFromList
    {
        public async static Task Execute(ISession session)
        {
            session.GUISettings.isSniping = true;
            Dictionary<PokemonId, PointLatLng> snipeList = new Dictionary<PokemonId, PointLatLng>();

            if (session.GUISettings.isStarted)
            {
                session.EventDispatcher.Send(new WarnEvent
                {
                    Message = "Starting to snipe list"
                });
                int count = 0, total = session.GUISettings.PokemonSnipeAuto.Count;
                foreach (var pokeSnipe in session.GUISettings.PokemonSnipeAuto.ToList())
                {
                    count++;
                    session.EventDispatcher.Send(new WarnEvent
                    {
                        Message = "Sniping " + count.ToString() + "/" + total.ToString()
                    });
                    snipeList.Clear();
                    snipeList.Add((PokemonId)Enum.Parse(typeof(PokemonId), pokeSnipe.Id.ToString()), new PointLatLng(pokeSnipe.Latitude, pokeSnipe.Longitude));
                    await SnipePokemonTask.AsyncStart(session, snipeList, default(CancellationToken));
                    session.GUISettings.PokemonSnipeAuto.Remove(pokeSnipe);
                }
            }
            session.GUISettings.isSniping = false;
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


                foreach (var pokemonToSnipe in pokemonIdList)
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

                        if (catchablePokemon.Count == 0)
                        {
                            session.EventDispatcher.Send(new ErrorEvent
                            {
                                Message = pokemonToSnipe.Key + " (" + pokemonToSnipe.Value.Lat.ToString() + "," + pokemonToSnipe.Value.Lng.ToString() + ") NOT FOUND"
                            });
                        }
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
