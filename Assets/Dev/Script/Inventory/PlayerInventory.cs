using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
   
    void Start()
    {
        inventorySize=20;
        InitializeSlots();
    }
}
