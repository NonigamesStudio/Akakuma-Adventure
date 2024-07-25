using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    
    [SerializeField] GameObject quickAccessInventory;
    
    void Awake()
    {
        inventorySize=22;   
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
