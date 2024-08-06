using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DialogueTriggerInfo : MonoBehaviour, Interactable
{
    [SerializeField] int idDialogue;
    [SerializeField] Transform t;
    public bool playedAlready = false;

    public InteractableType interactableType => InteractableType.NPC;

    public event Action OnCloseInteraction;

    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    public void Interact()
    {
        DialogueTest.instance.PlayDialogue(idDialogue, t);
        playedAlready=true;
    }


    private void OnEnable()
    {
        DialogueTest.OnStartDialogue +=() => { onDialogueStart?.Invoke(); }; 
        DialogueTest.OnEndDialogue += () => { onDialogueEnd?.Invoke(); }; ;
    }
}

