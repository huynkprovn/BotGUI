using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using PoGo.NecroBot.GUI.Utils;
using PoGo.NecroBot.Logic;
using PoGo.NecroBot.Logic.State;
using PoGo.NecroBot.Logic.Utils;
using POGOProtos.Map.Fort;
using POGOProtos.Map.Pokemon;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGo.NecroBot.GUI.Util
{ 
    public delegate void GUILiveMapDirtyDelegate();

    class GUILiveMap
    {
        public void Dirty(Session session)
        {
            UpdateLiveMap();
            DirtyEvent?.Invoke();
        }

        public async void UpdatePokeStopsGyms(Session session)
        {
            //var mapObjects = await session.Client.Map.GetMapObjects();

            //var pokeStopsGyms = mapObjects.Item1.MapCells.SelectMany(i => i.Forts)
            //    .Where(
            //        i =>
            //            (i.Type == FortType.Checkpoint || i.Type == FortType.Gym)
            //    ).ToList();
            if (session.GUISettings != null)
            {
                var pokeStopsGyms = session.GUISettings.CurrentPokestopList.ToList();

                if (pokeStopsGyms != null && pokeStopsGyms.Count() > 0)
                {
                    foreach (var stopToRemove in Bot._LiveMapPokeStops.ToList())
                    {
                        if (pokeStopsGyms.Where(p => p.Id == stopToRemove.Key && p.Type == FortType.Checkpoint).ToList().Count == 0)
                        {
                            Bot._LiveMapPokeStops.Remove(stopToRemove.Key);
                        }
                    }

                    foreach (var gymToRemove in Bot._LiveMapPokeGyms.ToList())
                    {
                        if (pokeStopsGyms.Where(p => p.Id == gymToRemove.Key && p.Type == FortType.Gym).ToList().Count == 0)
                        {
                            Bot._LiveMapPokeGyms.Remove(gymToRemove.Key);
                        }
                    }

                    foreach (var stop in pokeStopsGyms.ToList())
                    {
                        if (stop.Type == FortType.Checkpoint)
                        {
                            if (Bot._LiveMapPokeStops.ContainsKey(stop.Id) == false)
                                Bot._LiveMapPokeStops.Add(stop.Id, stop);
                        }
                        else
                        {
                            if (Bot._LiveMapPokeGyms.ContainsKey(stop.Id) == false)
                                Bot._LiveMapPokeGyms.Add(stop.Id, stop);
                        }
                    }
                }
            }
        }

        public async void UpdateMapPokemons(Session session)
        {
            //var mapObjects = await session.Client.Map.GetMapObjects();
            //var catchablePokemonsObj = mapObjects.Item1.MapCells.SelectMany(i => i.CatchablePokemons);

            if (session.GUISettings != null)
            {
                var catchablePokemonsObj = session.GUISettings.CurrentMapPokemonList;

                Dictionary<ulong, MapPokemon> mapPokemonList = new Dictionary<ulong, MapPokemon>();
                if (catchablePokemonsObj != null && catchablePokemonsObj.Count() > 0)
                {
                    // Make a list with catchable pokemons
                    foreach (var pokemon in catchablePokemonsObj.ToList())
                    {
                        mapPokemonList.Add(pokemon.EncounterId, pokemon);
                    }

                    // Delete
                    foreach (var pokemonToRemove in Bot._LiveMapPokemons.ToList())
                    {
                        if (mapPokemonList.Where(p => p.Key == pokemonToRemove.Key).ToList().Count == 0)
                        {
                            Bot._LiveMapPokemons.Remove(pokemonToRemove.Key);
                        }
                    }

                    // Add
                    foreach (var pokemon in mapPokemonList.ToList())
                    {
                        if (Bot._LiveMapPokemons.ContainsKey(pokemon.Key) == false)
                            Bot._LiveMapPokemons.Add(pokemon.Key, pokemon.Value);
                    }
                }
            }
        }

        private void UpdateLiveMap()
        {
            if (!Bot._Session.GUISettings.isSniping)
            {
                try
                {
                    // Position
                    if (Bot._LiveMapPositionUpdated)
                    {
                        Bot._LiveMapPositionUpdated = false;
                        Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.GUI.GoogleMap.Position = Bot._LiveMapCurrentPosition));
                        Bot.GUI.Invoke(new Action(() => Bot.GUI.LiveMapCurrentLatLng = Bot._LiveMapCurrentPosition.Lat.ToString() + "," + Bot._LiveMapCurrentPosition.Lng.ToString()));
                        Bot.mapOverlays["player"].Markers[0].Position = Bot._LiveMapCurrentPosition;

                        if (Bot._LiveMapLastPosition != null && (Bot._LiveMapLastPosition.Lat != 0 && Bot._LiveMapLastPosition.Lng != 0) && LocationUtils.CalculateDistanceInMeters(Bot._LiveMapCurrentPosition.Lat, Bot._LiveMapCurrentPosition.Lng, Bot._LiveMapLastPosition.Lat, Bot._LiveMapLastPosition.Lng) < 2000)
                        {
                            List<PointLatLng> polygon = new List<PointLatLng>();
                            polygon.Add(new PointLatLng(Bot._LiveMapLastPosition.Lat, Bot._LiveMapLastPosition.Lng));
                            polygon.Add(new PointLatLng(Bot._LiveMapCurrentPosition.Lat, Bot._LiveMapCurrentPosition.Lng));
                            GMapRoute route = new GMapRoute(polygon, "route");

                            route.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                            route.Stroke.Width = 2;
                            Bot.mapOverlays["path"].Routes.Add(route);
                        }
                    }

                    // PokeStops
                    var currentListPokestop = Bot.mapOverlays["pokestops"].Markers.ToList();

                    foreach (var line in currentListPokestop)
                    {
                        if (Bot._LiveMapPokeStops.Where(p => p.Key == (string)line.Tag).ToList().Count == 0)
                        {
                            Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.mapOverlays["pokestops"].Markers.Remove(line)));
                        }
                    }

                    Bitmap pokestopImg = new Bitmap(Bot.imagesList["pokestop"], new Size(20, 20));
                    pokestopImg.MakeTransparent(Color.White);

                    Bitmap pokestopluredImg = new Bitmap(Bot.imagesList["pokestop_lured"], new Size(20, 20));
                    pokestopluredImg.MakeTransparent(Color.White);

                    foreach (var pokestop in Bot._LiveMapPokeStops)
                    {
                        if (currentListPokestop.Where(p => (string)p.Tag == pokestop.Key).Count() == 0)
                        {
                            GMarkerGoogle marker;
                            marker = new GMarkerGoogle(new PointLatLng(pokestop.Value.Latitude, pokestop.Value.Longitude), pokestop.Value.LureInfo != null ? pokestopluredImg : pokestopImg);
                            marker.Tag = pokestop.Value.Id;
                            Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.mapOverlays["pokestops"].Markers.Add(marker)));
                        }
                    }

                    // Pokegyms
                    var currentListPokegyms = Bot.mapOverlays["pokegyms"].Markers.ToList();

                    foreach (var line in currentListPokegyms)
                    {
                        if (Bot._LiveMapPokeGyms.Where(p => p.Key == (string)line.Tag).ToList().Count == 0)
                        {
                            Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.mapOverlays["pokegyms"].Markers.Remove(line)));
                        }
                    }

                    Bitmap pokegymImg = new Bitmap(Bot.imagesList["pokegym"], new Size(20, 20));
                    pokestopImg.MakeTransparent(Color.White);

                    foreach (var pokegym in Bot._LiveMapPokeGyms)
                    {
                        if (currentListPokegyms.Where(p => (string)p.Tag == pokegym.Key).Count() == 0)
                        {
                            GMarkerGoogle marker;
                            marker = new GMarkerGoogle(new PointLatLng(pokegym.Value.Latitude, pokegym.Value.Longitude), pokegymImg);
                            marker.Tag = pokegym.Value.Id;
                            Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.mapOverlays["pokegyms"].Markers.Add(marker)));
                        }
                    }

                    // Pokemons
                    var currentListPokemons = Bot.mapOverlays["pokemons"].Markers.ToList();

                    foreach (var line in currentListPokemons)
                    {
                        if (Bot._LiveMapPokemons.Where(p => p.Key == (ulong)line.Tag).ToList().Count == 0)
                        {
                            Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.mapOverlays["pokemons"].Markers.Remove(line)));
                        }
                    }

                    foreach (var pokemon in Bot._LiveMapPokemons)
                    {
                        if (currentListPokemons.Where(p => (ulong)p.Tag == pokemon.Key).Count() == 0)
                        {
                            Bitmap pokemonImg = new Bitmap(40, 30);
                            Bot.imagesList.TryGetValue("pokemon_" + ((int)pokemon.Value.PokemonId).ToString(), out pokemonImg);

                            GMarkerGoogle marker;
                            marker = new GMarkerGoogle(new PointLatLng(pokemon.Value.Latitude, pokemon.Value.Longitude), pokemonImg);
                            marker.Tag = pokemon.Value.EncounterId;
                            Bot.GUI.GoogleMap.Invoke(new Action(() => Bot.mapOverlays["pokemons"].Markers.Add(marker)));
                        }
                    }
                }
                catch
                {
                    // error with livemap
                }
            }
        }

        public void SetPosition(PointLatLng position, Session session)
        {
            if (position != Bot._LiveMapCurrentPosition)
            {
                //UpdateMapPokemons(session);
                Bot._LiveMapPositionUpdated = true;
                Bot._LiveMapLastPosition = Bot._LiveMapCurrentPosition;
                Bot._LiveMapCurrentPosition = position;
            }
        }

        public event GUILiveMapDirtyDelegate DirtyEvent;
    }
}
