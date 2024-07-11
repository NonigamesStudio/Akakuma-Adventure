using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    
    [SerializeField] GameObject quickAccessInventory;
    void Start()
    {
        inventorySize=22;
        InitializeSlots();
    }
    public void HideQuickAccessInventory()
    {
        quickAccessInventory.SetActive(false);
    }
    public void ShowQuickAccessInventory()
    {
        quickAccessInventory.SetActive(true);
    }

}
