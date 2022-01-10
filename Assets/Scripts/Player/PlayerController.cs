using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Transform spawnPoint;
    public int sceneToLoad;
    public float moveSpeed;
    public LayerMask solidObjectLayer;
    public LayerMask grassLayer;
    public LayerMask interactableLayer;
    public LayerMask fovLayer;
    public LayerMask portalLayer;
    private bool isMoving;
    private bool changeSkin; //BBDD serveix per cambiar la skin
    public event Action<Collider2D> OnEnteredTrainersView;
    public event Action OnEncountered;
    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            changeSkin = true;
        }
        else
        {
            changeSkin = false;
        }
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("changeSkin", changeSkin);
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if(collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed*Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
        //posar a Handle Update a canvi de CheckForEncounters quan crei l'script de character
        OnMoveOver();
        
    }
    private void OnMoveOver()
    {
        CheckForEncounters();
        CheckIfTrainersView();
        CheckPortal();
        /*var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, 0.8f), 0.2f, GameLayers.i.TriggerableLayers);
        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }*/
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }


    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
           if (UnityEngine.Random.Range(1, 101) <= 10)
           {
               animator.SetBool("isMoving", false);
               Debug.Log("Encountered a wild pokemon");
               OnEncountered();
           }
        }
    }
    private void CheckPortal()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, portalLayer) != null)
        {
           Debug.Log("Player entered a portal");
           animator.SetBool("isMoving", false);
           StartCoroutine(SwitchScene());
        }
    }
    IEnumerator SwitchScene()
    {

        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        sceneToLoad = 0;

    }
    private void CheckIfTrainersView()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 0.2f, fovLayer);
        if (collider != null)
        {
            animator.SetBool("isMoving", false);
            OnEnteredTrainersView?.Invoke(collider);
        }
    }
    public string Name{
        get => name;
    }
}