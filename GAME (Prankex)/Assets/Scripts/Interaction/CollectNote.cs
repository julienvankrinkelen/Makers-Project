using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


// Not interactable to NPC anymore : used to pick up notes on the ground and immediately read them.
public class CollectNote : MonoBehaviour
{


    public NoteDialog NoteDialog;
    public PlayerCollectibles playerCollectibles;

    public bool isInRange;
    public bool canInteract = true;
    
    public SpriteRenderer spNote;
    public GameObject note;

    private void CollectTheNote()
    {
        spNote.enabled = false;
        NoteDialog.StartDialog();
        playerCollectibles.pickNote(note);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollectTheNote();
        }
    }
    
}
