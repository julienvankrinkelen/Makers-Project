using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableLantern : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerCollectibles playerCollectibles;
    [SerializeField] private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    [SerializeField] private DoorScript doorScript;

    private bool isInRange;
    private bool canInteract = true;
    
    [SerializeField] private GameObject Indicator;
    [SerializeField] private GameObject InteractText;

    private int LanternNumber;
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
        
        LanternNumber = parseCollectibleName(gameObject.name);
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
            && !playerCollectibles.lanternLightened[LanternNumber]
            && playerCollectibles.checkHasCandle())
        {
            playerCombat.anim.SetTrigger("UseCandle");
            playerCombat.SoundItemUsed();
            playerCombat.SoundLanternLighten();
            int currentLanternNumber = playerCollectibles.getLanternLightenedNumber(); // 0, 1 or 2 possible as the player is currently lighting another lantern
            Debug.Log("CURRENT LANTERN NUMBER = " + currentLanternNumber);
            doorScript.setCandleAnim(currentLanternNumber + 1);
            //Set the lantern number (00, 01 or 02) to TRUE in lanternLightened array (save / load mechanism)
            playerCollectibles.lightenLantern(this.transform.parent.gameObject, true);
            canInteract = false;
            playerCombat.nextAttackTime = Time.time + 1f / playerCombat.attackRate;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Player is in range of Interactable Lantern");

        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            Debug.Log("Player is not range of Interactable Lantern anymore");
            canInteract = true;
        }
    }

    public void SeeInteraction()
    {
        if (canInteract == true && isInRange == true && !playerCollectibles.lanternLightened[LanternNumber] && playerCollectibles.checkHasCandle())
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

    private int parseCollectibleName(string collectible)
    {
        return int.Parse(collectible.Substring(collectible.Length - 2));

    }
}
