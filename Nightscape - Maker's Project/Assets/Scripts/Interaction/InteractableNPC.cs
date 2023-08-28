using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


// Not interactable to NPC anymore : used to pick up notes on the ground and immediately read them.
public class InteractableNPC : MonoBehaviour
{

    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private NPCDialog NPCDialog;
    [SerializeField] private PlayerCollectibles playerCollectibles;
    [SerializeField] private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private bool isInRange;
    private bool canInteract = true;
    [SerializeField] private UnityEvent interactAction;

    [SerializeField] private GameObject Indicator;
    [SerializeField] private GameObject InteractText;
    private void Start()
    {
        InteractText.SetActive(false);

    }
    private void Awake()
    {

        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (isInRange
            && context.performed
            && canInteract == true)
        {
            canInteract = false;
            interactAction.Invoke();
        }

    }
    private void Update()
    {
        SeeInteraction();

    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range of NPC");

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is not range of NPC anymore");
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