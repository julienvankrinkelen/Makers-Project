using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    public bool isInRange;
    public bool canInteract = true;
    public UnityEvent interactAction;

    public GameObject Indicator;
    

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
        }
    }

    public void SeeInteraction()
    {
        if (canInteract == true && isInRange == true)
        {
            Indicator.SetActive(true);
        }
        else
        {
            Indicator.SetActive(false);
        }
    }
}