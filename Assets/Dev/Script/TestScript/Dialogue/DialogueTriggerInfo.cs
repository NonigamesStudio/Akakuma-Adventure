using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerInfo : MonoBehaviour, Interactable
{
    [SerializeField] int idDialogue;
    [SerializeField] Transform t;
    public bool playedAlready = false;

    public InteractableType interactableType => InteractableType.NPC;

    public event Action OnCloseInteraction;

    public void Interact()
    {
        DialogueTest.instance.PlayDialogue(idDialogue, t);
        playedAlready=true;
    }

}

