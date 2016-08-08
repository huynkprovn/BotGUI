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
using POGOProtos.Data;
using PoGo.NecroBot.GUI.Utils;
using System.Windows.Forms;
using System.Drawing;
using PoGo.NecroBot.Logic.PoGoUtils;

namespace PoGo.NecroBot.GUI.Util
{
    public delegate void GUIPokemonsDirtyDelegate();

    class GUIPokemons
    {
        //public Dictionary<ulong, PokemonData> _pokemons = new Dictionary<ulong, PokemonData>();

        public void SetPokemons(Inventory inventory)
        {
            if(inventory != null)
            {
                try
                {
                    var pokemons = inventory.GetPokemons().Result;
                    if (pokemons != null)
                    {
                        foreach (var pokemonToRemove in Bot.MyPokemons.ToList())
                        {
                            if (pokemons.Where(p => p.Id == pokemonToRemove.Key).ToList().Count == 0)
                            {
                                Bot.MyPokemons.Remove(pokemonToRemove.Key);
                            }
                        }

                        foreach (var pokemon in pokemons.ToList())
                        {
                            if (Bot.MyPokemons.ContainsKey(pokemon.Id))
                                Bot.MyPokemons[pokemon.Id] = pokemon;
                            else
                                Bot.MyPokemons.Add(pokemon.Id, pokemon);
                        }
                    }
                }
                catch
                {
                    // not loaded yet
                }
            }
        }

        public void AddPokemon(PokemonData pokemon)
        {
            PokemonData pokeData;
            if (Bot.MyPokemons.TryGetValue(pokemon.Id,out pokeData) == false)
                Bot.MyPokemons.Add(pokemon.Id, pokemon);
        }

        public void UpdatePokemons()
        {
            var currentPokemonList = Bot.GUI.DataGridMyPokemons.Rows.OfType<DataGridViewRow>().ToArray();

            foreach (var line in currentPokemonList)
            {
                if (Bot.MyPokemons.Where(p => p.Value.Id == (ulong)line.Cells[0].Value).ToList().Count == 0)
                {
                    Bot.GUI.DataGridMyPokemons.Invoke(new Action(() => Bot.GUI.DataGridMyPokemons.Rows.Remove(line)));
                }
            }

            foreach (var pokemon in Bot.MyPokemons)
            {
                if (currentPokemonList.Where(p => (ulong)p.Cells[0].Value == pokemon.Value.Id).Count() == 0)
                {
                    string power = pokemon.Value.IndividualAttack.ToString() + "a/" + pokemon.Value.IndividualDefense.ToString() + "d/" + pokemon.Value.IndividualStamina.ToString() + "s";
                    Bitmap evolve = new Bitmap(40, 30);
                    Bot.imagesList.TryGetValue("evolve", out evolve);
                    Bitmap transfer = new Bitmap(40, 30);
                    Bot.imagesList.TryGetValue("transfer", out transfer);
                    Bitmap bmp = new Bitmap(40, 30);
                    if (Bot.imagesList.TryGetValue("pokemon_" + ((int)pokemon.Value.PokemonId).ToString(), out bmp))
                        Bot.GUI.DataGridMyPokemons.Invoke(new Action(() => Bot.GUI.DataGridMyPokemons.Rows.Add(pokemon.Value.Id, bmp, pokemon.Value.PokemonId.ToString(), (int)pokemon.Value.PokemonId, pokemon.Value.Cp, PokemonInfo.CalculateMaxCp(pokemon.Value), Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon.Value), 1), PokemonInfo.GetLevel(pokemon.Value), pokemon.Value.Move1.ToString(), pokemon.Value.Move2.ToString(), power)));
                    else
                        Bot.GUI.DataGridMyPokemons.Invoke(new Action(() => Bot.GUI.DataGridMyPokemons.Rows.Add(pokemon.Value.Id, new Bitmap(40, 30), pokemon.Value.PokemonId.ToString(), (int)pokemon.Value.PokemonId, pokemon.Value.Cp, PokemonInfo.CalculateMaxCp(pokemon.Value), Math.Round(PokemonInfo.CalculatePokemonPerfection(pokemon.Value), 1), PokemonInfo.GetLevel(pokemon.Value), pokemon.Value.Move1.ToString(), pokemon.Value.Move2.ToString(), power)));
                }
            }

            Bot.GUI.Invoke(new Action(() => Bot.GUI.DataGridMyPokemonTab = "Pokemons (" + Bot.MyPokemons.Count().ToString() + "/" + Bot._ProfilePlayerMaxPokemonSpace.ToString() + ")"));
        }

        public void Dirty(Inventory inventory)
        {
            UpdatePokemons();
            DirtyEvent?.Invoke();
        }

        public event GUIPokemonsDirtyDelegate DirtyEvent;
    }
}
