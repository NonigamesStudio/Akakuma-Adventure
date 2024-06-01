using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryBaseClass : MonoBehaviour
{
    public int inventorySize;
    public int soulsCount;
    public List<ItemSO> items = new List<ItemSO>();
    public bool AddItem(ItemSO item)
    {
        if (items.Count < inventorySize)
        {
            items.Add(item);
            return true;
        }
        return false;
        
    }

    public void RemoveItem(ItemSO item)
    {
        items.Remove(item);
    }

    public void RemoveSouls(int souls)
    {
        soulsCount -= souls;
    }
}
