using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit; //To know if it is a player or an enemy
    [SerializeField] BattleHud hud;
    public Pokemon Pokemon { get; set; }
    Image image;
    Vector3 originalPos;
    Color originalColor;

    public bool IsPlayerUnit
    {
        get { return isPlayerUnit; }
    }

    public BattleHud Hud
    {
        get { return hud; }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }
    public void Setup(Pokemon pokemon)
    {
        this.Pokemon = pokemon;
        //Assigns the pokemon of the player
        
        if (isPlayerUnit)
            image.sprite = pokemon.Base.BackSprite;
        else
            image.sprite = pokemon.Base.FrontSprite;

        hud.SetData(pokemon);
        hud.gameObject.SetActive(true);
        PlayEnterAnimation();
    }
    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }
        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));

        sequence.Append(image.DOColor(originalColor, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y -150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f)); //to make it invisible
    }

    public void Clear()
    {
        hud.gameObject.SetActive(false);
    }
}
