using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]

public class PokemonBase : ScriptableObject
{
    [SerializeField] string name; //BBDD, la resta ja esta dins del unity
    [TextArea]
    [SerializeField] string description; //Nose si la necessitem pero esta xula 
    [SerializeField] Sprite frontSprite; 
    [SerializeField] Sprite backSprite; 
    [SerializeField] PokemonType type;
    

    //Base Stats    
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed; //We need it if we want to calculate maxHp in the same way that the original game does
    
    //Pokemon stored attacks
    [SerializeField] List<LearnableMove> learnableMoves;

    public int getMaxHP()
    {
        return maxHp;
    }
    public string Name{
        get { return name; } //How to expose a private variable
    }
    public string Description{
        get { return description; }
    }
    public Sprite FrontSprite{
        get { return frontSprite; }
    }
    public Sprite BackSprite{
        get { return backSprite; }
    }
    public PokemonType Type
    {
        get { return type; }
    }
    public int MaxHp
    {
        get { return maxHp; }
    }
    public int Attack
    {
        get { return attack; }
    }
    public int Defense
    {
        get { return defense; }
    }
    public int Speed
    {
        get { return speed; }
    }
    public int SpAttack
    {
        get { return spAttack; }
    }
    public int SpDefense
    {
        get { return spDefense; }
    }
    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base
    {
        get { return moveBase; }
    }
    public int Level
    {
        get { return level; }
    }
}

public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Grass,
    Ice,
    Rock,
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // These 2 are not actual stats, they're used to boost the moveAccuracy
    Accuracy,
    Evasion
}

public class TypeChart
{
    static float[][] chart = 
    {
        //                   NOR  FIR WAT GRS ICE RCK
        /*NOR*/ new float[] { 1f, 1f, 1f, 1f, 1f, 1f }, //none
        /*FIR*/ new float[] { 1f, 1f, 1f, 2f, 2f, 1f }, //ice, grass
        /*WAT*/ new float[] { 1f, 2f, 1f, 1f, 1f, 2f }, //rock, fire
        /*GRS*/ new float[] { 1f, 1f, 2f, 1f, 1f, 2f }, //water, rock
        /*ICE*/ new float[] { 1f, 1f, 2f, 2f, 1f, 1f }, //water, grass
        /*RCK*/ new float[] { 1f, 2f, 1f, 1f, 2f, 1f }  //ice, fire
    };
    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (attackType == PokemonType.None || defenseType == PokemonType.None)
            return 1;
        int row = (int)attackType -1;
        int col = (int)defenseType -1;
        return chart[row][col];
    }
}
