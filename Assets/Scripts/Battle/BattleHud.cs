using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text statusText;

    [SerializeField] Color psnColor;
    [SerializeField] Color brnColor;
    [SerializeField] Color slpColor;
    [SerializeField] Color parColor;
    [SerializeField] Color frzColor;
    Pokemon _pokemon;
    Dictionary<ConditionID, Color> statusColors;
    public void SetData(Pokemon pokemon)
    {
        nameText.text = pokemon.Base.Name;
        levelText.text = "Lvl" + pokemon.Level;
        _pokemon = pokemon;
        float calc = (float)pokemon.HP / pokemon.MaxHp;
        hpBar.SetHP(calc);
        statusColors = new Dictionary<ConditionID, Color>()
        {
            {ConditionID.psn, psnColor },
            {ConditionID.brn, brnColor },
            {ConditionID.slp, slpColor },
            {ConditionID.par, parColor },
            {ConditionID.frz, frzColor },
        };
        SetStatusText();
        _pokemon.OnStatusChanged += SetStatusText;
    }
    void SetStatusText()
    {
        if (_pokemon.Status == null)
        {
            statusText.text = "";
        }
        else
        {
            statusText.text = _pokemon.Status.Id.ToString().ToUpper();
            statusText.color = statusColors[_pokemon.Status.Id];
        }
    }
    public IEnumerator UpdateHP()
    {
        if (_pokemon.HpChanged == true)
        {
            float calc = (float)_pokemon.HP / _pokemon.MaxHp;
            yield return hpBar.SetHPSmooth(calc);
            _pokemon.HpChanged = false;
        }


    }
}
