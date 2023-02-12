using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public KeyCode interactKey;
    public bool isInRange;
    public UnityEvent interactAction;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isInRange && Input.GetKeyDown(interactKey))
        {
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
        }
    }
}
