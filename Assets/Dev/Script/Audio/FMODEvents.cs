using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Reflection;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player Sounds")]
    [field: SerializeField] public EventReference wooshSound { get; private set; }
    [field: SerializeField] public EventReference dashSound { get; private set; }
    [field: SerializeField] public EventReference changeWeapon { get; private set; }
    [field: Header("Music")]
    [field: SerializeField] public EventReference menuMusic { get; private set; }
    
    public static FMODEvents instance;

    void Awake()
    {
        instance = this;
    }
}
