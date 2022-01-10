using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons
    {
        get { return this.pokemons; }
    }

    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }
    public Pokemon GetHealthyPokemon()
    {
        //Look through the list of pokemons that we have, and returns the first pokemon that satisfies the condition
        //If the condition isn't satisfied returns null
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
 
    }
}
