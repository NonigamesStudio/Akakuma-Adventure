using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerInfo : MonoBehaviour, Interactable
{
    [SerializeField] int idDialogue;
    [SerializeField] Transform t;

    public InteractableType interactableType => throw new NotImplementedException();

    public event Action OnCloseInteraction;

    public void Interact()
    {
        DialogueTest.instance.PlayDialogue(idDialogue, t);
    }

}
