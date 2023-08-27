using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractableStatue : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    [SerializeField] private PlayerCombat playerCombat;

    private bool isInRange;
    private bool canInteract = true;
    [SerializeField] private UnityEvent interactAction;

    [SerializeField] private GameObject Indicator;
    [SerializeField] private GameObject InteractText;
    [SerializeField] private GameObject panel;

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

    private void Update()
    {
        SeeInteraction();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (isInRange && context.performed && canInteract == true)
        {
            canInteract = false;
            interactAction.Invoke();
            playerCombat.SoundOpenStatue();
            playerCombat.EnableCombat(false);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range of interactable object");

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is not range of interactable object anymore");
            canInteract = true;
            panel.SetActive(false);
            playerCombat.EnableCombat(true);
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
