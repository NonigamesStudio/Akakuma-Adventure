using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public event Action OnItemListChange;
    public int inventorySize=20;
    public int soulsCount;
    public List<ItemSlot> items = new List<ItemSlot>();
    
    [SerializeField] ItemSO healthPotion;

    void OnEnable()
    {
        Coin.OnCoinCollected += UpdateCoins;
        InitializeSlots();
    }
    void OnDisable()
    {
        Coin.OnCoinCollected -= UpdateCoins;
    }
    public void InitializeSlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlot(null, i));
        }
    }
    private void UpdateCoins()
    {
        soulsCount++;
    }

    public virtual bool AddItem(ItemSO item)
    {
        if (CheckFreeSlot())
        {
            foreach(ItemSlot itemSlot in items)
            { 
                if (itemSlot.item == null)
                {
                    itemSlot.item = item;
                    break;
                }
            }
            item.inventory=this;

            OnItemListChange?.Invoke();
            return true;
        }
        return false;
    }

    public virtual void UseItem(int slot)
    {
        items[slot].item.Use(gameObject);
        RemoveItem(slot);
    }

    public virtual void RemoveItem(int slot)
    {
        foreach (ItemSlot itemSlot in items)
        {
            if (itemSlot.slotNumber == slot)
            {
                itemSlot.item = null;
            }
        }
        OnItemListChange?.Invoke();
    }

    public void RemoveSouls(int souls)
    {
        soulsCount -= souls;
    }

    public bool CheckFreeSlot()
    {
        foreach (ItemSlot itemSlot in items)
        {
            if (itemSlot.item == null)
            {
                return true;
            }
        } 
        return false;
    }

    [ContextMenu("Add Health Potion")]
    public void AddHealthPotion()
    {
        AddItem(healthPotion);
    }   



}

[System.Serializable]
public class ItemSlot
{
    public ItemSO item;
    public int slotNumber;
    public ItemSlot(ItemSO item, int slotNumber)
    {
        this.item = item;
        this.slotNumber = slotNumber;
    }
}
