using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoGo.NecroBot.Logic.Common;
using PoGo.NecroBot.Logic.Event;
using PoGo.NecroBot.Logic.PoGoUtils;
using PoGo.NecroBot.Logic.State;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;
using Quobject.SocketIoClientDotNet.Client;
using PoGo.NecroBot.Logic.Logging;
using PoGo.NecroBot.GUI.Utils;
using System.Drawing;
using System.Windows.Forms;

namespace PoGo.NecroBot.GUI.Tasks
{
    class GetPokemonToSnipePokeZZ
    {

        public class PokeZZWebReader
        {
            public static PokeZZWebReader _pokezzWebReader;

            public PokeZZWebReader()
            {
                InitializeWebClient();
            }

            public void InitializeWebClient()
            {
                 Logger.Write($"Connection established to PokeZZ. Waiting for data...", LogLevel.Warning);
            }

            public async static Task<Task> StartPollPokeZZFeed(ISession session)
            {
                return await Task.Factory.StartNew(async () => await PokeZZThread(session), TaskCreationOptions.LongRunning);
            }

            public async static Task PokeZZThread(ISession session)
            {
                const int delay = 10 * 1000;
                while (true)
                {
                    for (var retrys = 0; retrys <= 3; retrys++)
                    {
                        List<SniperInfo> SnipeLocations = new List<SniperInfo>();

                        var options = new IO.Options();
                        options.Transports = Quobject.Collections.Immutable.ImmutableList.Create<string>("websocket");

                        var socket = IO.Socket("http://pokezz.com", options);

                        var hasError = true;

                        ManualResetEventSlim waitforbroadcast = new ManualResetEventSlim(false);

                        List<PokemonLocation_pokezz> pokemons = new List<PokemonLocation_pokezz>();

                        socket.On("a", (msg) =>
                        {
                            hasError = false;
                            socket.Close();
                            string[] pokemonDefinitions = ((String)msg).Split('~');

                            foreach (var pokemonDefinition in pokemonDefinitions)
                            {
                                try
                                {
                                    string[] pokemonDefinitionElements = pokemonDefinition.Split('|');
                                    PokemonLocation_pokezz pokezzElement = new PokemonLocation_pokezz();
                                    pokezzElement.name = (PokemonId)Convert.ToInt32(pokemonDefinitionElements[0], CultureInfo.InvariantCulture);
                                    pokezzElement.lat = Convert.ToDouble(pokemonDefinitionElements[1], CultureInfo.InvariantCulture);
                                    pokezzElement.lng = Convert.ToDouble(pokemonDefinitionElements[2], CultureInfo.InvariantCulture);
                                    pokezzElement.time = Convert.ToDouble(pokemonDefinitionElements[3], CultureInfo.InvariantCulture);
                                    pokezzElement.verified = (pokemonDefinitionElements[4] == "0") ? false : true;
                                    pokezzElement.iv = pokemonDefinitionElements[5];
                                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(pokezzElement.time).ToLocalTime();
                                    if (pokezzElement.verified && dateTime > DateTime.Now)
                                        pokemons.Add(pokezzElement);
                                }
                                catch (Exception)
                                {
                                    // Just in case Pokezz changes their implementation, let's catch the error and set the error flag.
                                    hasError = true;
                                }
                            }
                            waitforbroadcast.Set();
                        });

                        socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_ERROR, () =>
                        {
                            socket.Close();
                            hasError = true;
                            waitforbroadcast.Set();
                        });

                        socket.On(Quobject.SocketIoClientDotNet.Client.Socket.EVENT_CONNECT_ERROR, () =>
                        {
                            socket.Close();
                            hasError = true;
                            waitforbroadcast.Set();
                        });

                        waitforbroadcast.Wait(5000);
                        socket.Close();
                        if (!hasError)
                        {
                            if (Bot._Session.GUISettings.PokemonSnipeCaught.Count > 0)
                            {
                                foreach (var pokemon in Bot._Session.GUISettings.PokemonSnipeCaught.ToList())
                                {
                                    Bitmap bmp = new Bitmap(40, 30);
                                    Bot.imagesList.TryGetValue("pokemon_" + ((int)pokemon.PokemonId).ToString(), out bmp);
                                    Bot.GUI.DataGridSnipeCaught.Invoke(new Action(() => Bot.GUI.DataGridSnipeCaught.Rows.Add(bmp, pokemon.PokemonId.ToString(), pokemon.Cp, PokemonInfo.CalculateMaxCp(pokemon), Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon)), PokemonInfo.GetLevel(pokemon), DateTime.Now)));
                                    try
                                    {
                                        Bot._Session.GUISettings.PokemonSnipeCaught.Remove(pokemon);
                                    }
                                    catch(Exception ex)
                                    {
                                        Logger.Write("Error caught: " + ex.Message, LogLevel.Warning);
                                    }
                                    
                                }
                            }

                            foreach (var pokemon in pokemons)
                            {
                                var SnipInfo = new SniperInfo();
                                SnipInfo.Id = pokemon.name;
                                SnipInfo.Latitude = pokemon.lat;
                                SnipInfo.Longitude = pokemon.lng;
                                //SnipInfo.TimeStampAdded = DateTime.Now;
                                SnipInfo.ExpirationTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(pokemon.time).ToLocalTime();
                                SnipInfo.IV = pokemon._iv;
                                if (pokemon.verified && SnipInfo.ExpirationTimestamp > DateTime.Now)
                                    SnipeLocations.Add(SnipInfo);

                                // Check name + lat/lng so we don't add same pokemon twice and not add Missingno
                                if ((Bot.PokemonSnipeFeed.Where(p => p.Id == SnipInfo.Id && Math.Round(p.Latitude, 5) == Math.Round(SnipInfo.Latitude, 5) && Math.Round(p.Longitude, 5) == Math.Round(SnipInfo.Longitude, 5)).Count() == 0 && SnipInfo.Id != PokemonId.Missingno) &&
                                    (Bot.PokemonSnipeFeedDeleted.Where(p => p.Id == SnipInfo.Id && Math.Round(p.Latitude, 5) == Math.Round(SnipInfo.Latitude, 5) && Math.Round(p.Longitude, 5) == Math.Round(SnipInfo.Longitude, 5)).Count() == 0 && SnipInfo.Id != PokemonId.Missingno))
                                {
                                    if (Bot.GUI.AutoSnipe || Bot.GUI.AutoSnipeAll)
                                    {
                                        if ((Bot._Session.LogicSettings.PokemonToSnipe.Pokemon.Contains(SnipInfo.Id) && SnipInfo.IV >= Bot.GUI.MinSnipeIV) || Bot.GUI.AutoSnipeAll)
                                        {
                                            Logger.Write("Auto Sniping (PokeZZ): " + SnipInfo.ToString(), LogLevel.Warning);
                                            Logic.Tasks.SniperInfo pokeSnipeInfo = new Logic.Tasks.SniperInfo();
                                            pokeSnipeInfo.Id = SnipInfo.Id;
                                            pokeSnipeInfo.IV = SnipInfo.IV;
                                            pokeSnipeInfo.Latitude = SnipInfo.Latitude;
                                            pokeSnipeInfo.Longitude = SnipInfo.Longitude;
                                            Bot._Session.GUISettings.PokemonSnipeAuto.Add(pokeSnipeInfo);
                                            Bot.PokemonSnipeFeedDeleted.Add(SnipInfo);
                                        }
                                        else
                                        {
                                            int sort = Bot.GUI.DataGridSnipePokemons.Rows.Count + 1;
                                            Bitmap bmp = new Bitmap(40, 30);
                                            Bot.imagesList.TryGetValue("pokemon_" + ((int)SnipInfo.Id).ToString(), out bmp);
                                            Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Rows.Add(bmp, SnipInfo.Id, SnipInfo.IV, SnipInfo.Latitude, SnipInfo.Longitude, SnipInfo.ExpirationTimestamp, "Snipe!", sort, SnipInfo.EncounterId)));
                                            Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Sort(Bot.GUI.DataGridSnipePokemons.Columns["dataSnipingFeederColTimestamp"], System.ComponentModel.ListSortDirection.Descending)));
                                            Bot.PokemonSnipeFeed.Add(SnipInfo);
                                        }
                                    }
                                    else {
                                        int sort = Bot.GUI.DataGridSnipePokemons.Rows.Count + 1;
                                        Bitmap bmp = new Bitmap(40, 30);
                                        Bot.imagesList.TryGetValue("pokemon_" + ((int)SnipInfo.Id).ToString(), out bmp);
                                        Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Rows.Add(bmp, SnipInfo.Id, SnipInfo.IV, SnipInfo.Latitude, SnipInfo.Longitude, SnipInfo.ExpirationTimestamp, "Snipe!", sort, SnipInfo.EncounterId)));
                                        Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Sort(Bot.GUI.DataGridSnipePokemons.Columns["dataSnipingFeederColTimestamp"], System.ComponentModel.ListSortDirection.Descending)));
                                        Bot.PokemonSnipeFeed.Add(SnipInfo);
                                        //Logger.Write(msg.ToString(), LogLevel.Warning);
                                    }

                                    // Remove pokemons that have expired
                                    foreach (var pokemonToRemove in Bot.PokemonSnipeFeed.ToList())
                                    {
                                        if (pokemonToRemove.ExpirationTimestamp < DateTime.Now)
                                        {
                                            Bot.PokemonSnipeFeed.Remove(pokemonToRemove);
                                            Bot.PokemonSnipeFeedDeleted.Add(pokemonToRemove);
                                            var row = Bot.GUI.DataGridSnipePokemons.Rows.Cast<DataGridViewRow>().FirstOrDefault(p => (PokemonId)p.Cells["dataSnipingFeederColName"].Value == pokemonToRemove.Id && (double)p.Cells["dataSnipingFeederColLat"].Value == pokemonToRemove.Latitude && (double)p.Cells["dataSnipingFeederColLng"].Value == pokemonToRemove.Longitude);
                                            if (row != null)
                                            {
                                                try
                                                {
                                                    Bot.GUI.DataGridSnipePokemons.Invoke(new Action(() => Bot.GUI.DataGridSnipePokemons.Rows.Remove(row)));
                                                }
                                                catch(Exception ex) {
                                                    Logger.Write("Error remove: "+ex.Message, LogLevel.Warning);
                                                }
                                            }

                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            session.EventDispatcher.Send(new ErrorEvent { Message = "(Pokezz.com) Connection Error" });
                        }

                        Task.Delay(delay);
                    }
                }
            }

            public async static Task<Task> TryStartPokeZZReader(ISession session)
            {
                while (true)
                {
                    _pokezzWebReader = new PokeZZWebReader();
                    try
                    {
                        return await StartPollPokeZZFeed(session);
                    }
                    catch(Exception ex)
                    {
                        Logger.Write($"Unknown exception " + ex, LogLevel.Warning);
                        continue;
                    }
                    finally
                    {
                        Task.Delay(20 * 1000);
                    }
                }
            }

            public class PokemonLocation_pokezz
            {

                public double time { get; set; }
                public double lat { get; set; }
                public double lng { get; set; }
                public int id { get; set; }
                public string iv { get; set; }
                public double _iv
                {
                    get
                    {
                        try
                        {
                            return Convert.ToDouble(iv);
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                }
                public PokemonId name { get; set; }
                public string[] skills { get; set; }
                public Boolean verified { get; set; }

            }
        }
    }
}
