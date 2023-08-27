using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class CollectNote : MonoBehaviour
{
    [SerializeField] private NoteDialog NoteDialog;
    [SerializeField] private PlayerCollectibles playerCollectibles;

    [SerializeField] private bool isInRange;
    [SerializeField] private bool canInteract = true;
    
    [SerializeField] private SpriteRenderer spNote;
    [SerializeField] private GameObject note;

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
