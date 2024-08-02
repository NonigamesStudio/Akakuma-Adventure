using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    [SerializeField] InventorySO inventoryData;
    public Action OnItemListChange;
    public Action<Inventory> OnItemListChangeToSO;
    public int inventorySize=20;
    public int soulsCount;
    public List<ItemSlot> items = new List<ItemSlot>();
    [SerializeField] PortalTicket portalTicket;
    

    void OnEnable()
    {
        Coin.OnCoinCollected += UpdateCoins;
        OnItemListChangeToSO += inventoryData.CopyItemsFromInventory;
        LeanTween.delayedCall(0.1f, () => { GameManager.instance.OnSceneChange += WriteSO; });
        InitializeSlots();
    }
    void OnDisable()
    {
        Coin.OnCoinCollected -= UpdateCoins;
        OnItemListChangeToSO -= inventoryData.CopyItemsFromInventory;
        GameManager.instance.OnSceneChange -= WriteSO;
    }
    public void InitializeSlots()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlot(null, i));
        }
        if (items.Count==inventorySize)
        {
            LeanTween.delayedCall(0.1f, () => { CopyItemsFromSO(inventoryData); });
            //CopyItemsFromSO(inventoryData);
        }
        
    }
    private void UpdateCoins()
    {
        soulsCount++;
        OnItemListChange?.Invoke();
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

    public void AddSouls(int souls)
    {
        soulsCount += souls;
        OnItemListChange?.Invoke();
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

   public void SwapItems(int slotOriginIndex, int slotIndexFinal)
   {
        foreach (ItemSlot itemSlot in items)
        {
            if (itemSlot.slotNumber == slotIndexFinal && items[slotIndexFinal].item==null)
            {
                items[slotIndexFinal].item = items[slotOriginIndex].item;
                items[slotOriginIndex].item = null;
                OnItemListChange?.Invoke();
            }
        }
   }

    private void WriteSO()
    {
        OnItemListChangeToSO?.Invoke(this);
    }

    public void CopyItemsFromSO(InventorySO inventoryData)
    {
       
        if (inventoryData == null) return;
       
        if (soulsCount != inventoryData.soulsCount) soulsCount = inventoryData.soulsCount;
        

        if (inventoryData.items != null && inventoryData.items.Length == items.Count)
        {
            
            for (int i = 0; i < inventoryData.items.Length; i++)
            {
                
                if (inventoryData.items[i] != null)
                {
                    items[i].item = inventoryData.items[i].item;
                    
                }
                else
                {
                    items[i].item = null;
                }
            }
        }else{
           
            Debug.LogError("Para " + this.ToString()+ "El inventario tiene "+items.Count+" slots y el SO tiene "+inventoryData.items.Length+" slots");
        }
        //agrega portal tierra si no tiene despues de copiar del SO
        if (!(this is PlayerInventory))
        {
            bool hasPortalTicket = false;
            foreach (ItemSlot itemSlot in items)
            {
                
                if (itemSlot.item is PortalTicket)
                {
                    hasPortalTicket = true;
                    break;
                }
            }
            if (!hasPortalTicket)
            {
                AddItem(portalTicket);
            }
        }
        OnItemListChange?.Invoke();
    }

    [ContextMenu("Save Inventory")]
    void SaveInventory()
    {
        inventoryData.CopyItemsFromInventory(this);
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