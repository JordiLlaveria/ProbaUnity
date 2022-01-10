using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    //This script will store all the wild pokemons on the area
    [SerializeField] List<Pokemon> wildPokemons;

    public Pokemon GetWildPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0,wildPokemons.Count)];
        wildPokemon.Init();
        return wildPokemon;
    }
}
