using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Interactable
{
    event Action OnCloseInteraction;
    public InteractableType interactableType { get; }
    public void Interact();
}

public enum InteractableType
{
    NPC,
    Shop,
   
}

