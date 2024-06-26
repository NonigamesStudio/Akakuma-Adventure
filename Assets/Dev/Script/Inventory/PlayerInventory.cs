using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] ItemSO item;
    
    void Start()
    {
        inventorySize=20;
        InitializeSlots();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItem(item);
        }
    }

}
