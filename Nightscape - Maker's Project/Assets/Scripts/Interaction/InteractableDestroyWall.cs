using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableDestroyWall : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerCollectibles playerCollectibles;
    [SerializeField] private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private bool isInRange;
    private bool canInteract = true;
    private bool pressedDestroy = false;
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
            && playerCollectibles.getExplosiveScrollNumber()>=1)
        {
            playerCombat.anim.SetTrigger("UseScroll");
            playerCombat.SoundItemUsed();
            playerCombat.SoundWallDestroy();
            playerCollectibles.removeExplosiveScroll();
            canInteract = false;
            pressedDestroy = true;
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
        if (canInteract == true && isInRange == true && !pressedDestroy && playerCollectibles.getExplosiveScrollNumber() >= 1)
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