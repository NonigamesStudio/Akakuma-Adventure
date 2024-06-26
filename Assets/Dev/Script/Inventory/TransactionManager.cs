using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;


public class TransactionManager : MonoBehaviour
{
    public Inventory shopInventory=null;
    public Inventory playerInventory;
    [SerializeField] TransactionUI transactionUI;
    int transactionListSize = 4;
    [SerializeField]List<ItemSlot> itemsInTransactionPlayer; 
    [SerializeField]List<ItemSlot> itemsInTransactionShop;
    [SerializeField] ItemSO item;
    [SerializeField] ItemSO item2;
    
    
    void UpdateUI()
    {
        List<List<ItemSlot>> itemsInInventory = new List<List<ItemSlot>>();

        #region create a list with elements in itemsInTransactionPlayer and itemsInTransactionShop
        
        int totalSize = 2 * System.Math.Max(itemsInTransactionPlayer.Count, itemsInTransactionShop.Count);

        List<ItemSlot> itemsInTransaction = new List<ItemSlot>(new ItemSlot[totalSize]);

        for (int i = 0; i < itemsInTransactionShop.Count; i++)
        {

            itemsInTransaction[i*2]= itemsInTransactionShop[i];
             
        }
        for (int i = 0; i < itemsInTransactionPlayer.Count; i++)
        {
            itemsInTransaction[i*2+1]= itemsInTransactionPlayer[i];
        }
        #endregion
        if (shopInventory != null)
        {

            itemsInInventory.Add(shopInventory.items);
            
        }
        else
        {
            itemsInInventory.Add(null);
        }

        itemsInInventory.Add(itemsInTransaction);
        itemsInInventory.Add(playerInventory.items);

        transactionUI.UpdateInventory(itemsInInventory);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (CheckFreeSloInList(itemsInTransactionShop))
            {
                foreach (ItemSlot itemSlot in itemsInTransactionShop)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = item;
                        break;
                    }
                }
            }
            UpdateUI();
        }
         if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (CheckFreeSloInList(itemsInTransactionPlayer))
            {
                foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = item;
                        break;
                    }
                }
            }
            UpdateUI();
        }
    }

    public void InitializeTransaction(Inventory shopInventory)
    {
        
        this.shopInventory = shopInventory;
        List<ItemSlot> emptyList1=new List<ItemSlot>();
        List<ItemSlot> emptyList2=new List<ItemSlot>();
        for (int i = 0; i < transactionListSize; i++)
        {
            emptyList1.Add(new ItemSlot (null, i));
            emptyList2.Add(new ItemSlot (null, i));
        }
        itemsInTransactionPlayer = emptyList1;
        itemsInTransactionShop = emptyList2;
        UpdateUI();
    }

    public void MoveItem(int slot, int list)
    {
        if (list == 0)
        {
            
            if (CheckFreeSloInList(itemsInTransactionShop))
            {
                foreach (ItemSlot itemSlot in itemsInTransactionShop)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = item;
                        break;
                    }
                }
                foreach (ItemSlot itemSlot in shopInventory.items)
                {
                    if (itemSlot.slotNumber == slot)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }
            
        }
        else if (list == 1)//De transaccion a shop
        {

           

            if (playerInventory.CheckFreeSlot())
            {
                
                foreach (ItemSlot itemSlot in shopInventory.items)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = item;
                        break;
                    }
                }

                foreach (ItemSlot itemSlot in itemsInTransactionShop)
                {
                    if (itemSlot.slotNumber == slot/2)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }
            
        }
        else if (list == 2)//De la transaccion al player
        {

            
            
            if (playerInventory.CheckFreeSlot())
            {
                
                foreach (ItemSlot itemSlot in playerInventory.items)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = item;
                        break;
                    }
                }

                foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
                {
                    if (itemSlot.slotNumber == slot/2)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }
        }
        else if (list == 3)
        {
            if (playerInventory == null) return;
            
            if (CheckFreeSloInList(itemsInTransactionPlayer))
            {
                foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = item;
                        break;
                    }
                }
                foreach (ItemSlot itemSlot in playerInventory.items)
                {
                    if (itemSlot.slotNumber == slot)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }
           
        }

        UpdateUI();
    }

    private bool CheckFreeSloInList(List<ItemSlot> list)
    {
        foreach (ItemSlot itemSlot in list)
        {
            if (itemSlot.item == null)
            {
                return true;
            }
        } 
        return false;
    }
    

}
