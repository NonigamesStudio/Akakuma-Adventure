using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public event Action OnItemListChange;
    public int inventorySize;
    public int soulsCount;
    public List<ItemSO> items = new List<ItemSO>();
    [SerializeField] ItemSO healthPotion;

    void OnEnable()
    {
        Coin.OnCoinCollected += UpdateCoins;
    }
    void OnDisable()
    {
        Coin.OnCoinCollected -= UpdateCoins;
    }

    private void UpdateCoins()
    {
        soulsCount++;
    }

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

    [ContextMenu("Add Health Potion")]
    public void AddHealthPotion()
    {
        AddItem(healthPotion);
    }   

}
