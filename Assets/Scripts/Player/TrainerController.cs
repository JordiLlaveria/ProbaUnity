using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour, Interactable
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] GameObject exclamation;
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialogAfter;
    public GameObject fovLayer;
    bool battleLost = false;
    /*Character character;
    private void Awake()
    {
        character = GetComponent<Character>();
    }*/
    public void Interact()
    {
        
        if(!battleLost)
        {
            GameController.Instance.StartTrainerBattle(this);
        }
        else
        {
            StartCoroutine(DialogManager.Instance.ShowDialog(dialogAfter));
        }
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);


        //var diff = player.transform.position - transform.position;
        //var moveVec = diff - diff.normalized;
        //moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));
        //yield return character.Move(moveVec);

        /*StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => 
        {
            GameController.Instance.StartTrainerBattle(this);
        }));*/
       
        //StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        yield return new WaitForSeconds(1f);
        GameController.Instance.StartTrainerBattle(this);

    }
    public void BattleLost()
    {
        fovLayer.gameObject.SetActive(false);
        battleLost = true;
    }

    public string Name{
        get => name;
    }
    public Sprite Sprite{
        get => sprite;
    }
}
