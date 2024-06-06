using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public event Action OnItemListChange;
    public int inventorySize;
    public int soulsCount;
    [NonSerialized]public List<ItemSO> items = new List<ItemSO>();
   
    public virtual bool AddItem(ItemSO item)
    {
        if (items.Count < inventorySize)
        {
            items.Add(item);
            item.inventory=this;
            OnItemListChange?.Invoke();
            return true;
        }
        return false;
        
    }

    public virtual void RemoveItem(ItemSO item)
    {
        items.Remove(item);
        OnItemListChange?.Invoke();
    }

    public void RemoveSouls(int souls)
    {
        soulsCount -= souls;
    }

}