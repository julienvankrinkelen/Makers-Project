using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableLantern : MonoBehaviour
{
    public PlayerCombat playerCombat;
    public PlayerCollectibles playerCollectibles;
    public PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    public bool isInRange;
    public bool canInteract = true;
    public UnityEvent interactAction;

    public GameObject Indicator;
    public GameObject InteractText;

    private void Start()
    {
        InteractText.SetActive(false);

    }
    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Object.performed += Object;
    }

    private void Update()
    {
        SeeInteraction();
    }

    public void Object(InputAction.CallbackContext context)
    {
        // If player in range, if pressed button, if hasn't pressed already, if cooldown up, of selected scroll (and not candle), if player has scrolls
        if (isInRange
            && context.performed
            && canInteract == true
            && Time.time >= playerCombat.nextAttackTime
            //&& playerCombat.CandleSelected
            && playerCollectibles.checkHasCandle())
        {
            // playerCombat.anim.SetTrigger("UseCandle");
            canInteract = false;
            interactAction.Invoke();
            playerCombat.nextAttackTime = Time.time + 1f / playerCombat.attackRate;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range of destructible Wall");

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is not range of destructible wall anymore");
            canInteract = true;
        }
    }

    public void SeeInteraction()
    {
        if (canInteract == true && isInRange == true)
        {
            Indicator.SetActive(true);
            InteractText.SetActive(true);


        }
        else
        {
            InteractText.SetActive(false);
            Indicator.SetActive(false);

        }
    }
}
