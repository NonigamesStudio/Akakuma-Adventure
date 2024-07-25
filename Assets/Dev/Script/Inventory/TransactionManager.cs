using System;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TransactionManager : MonoBehaviour
{
    public static event Action OnTransactionEnds;
    public Inventory shopInventory=null;
    public Inventory playerInventory;
    [SerializeField] TransactionUI transactionUI;
    int transactionListSize = 4;
    [SerializeField] UIManager uiManager;
    [SerializeField]List<ItemSlot> itemsInTransactionPlayer; 
    [SerializeField]List<ItemSlot> itemsInTransactionShop;
    [SerializeField] ItemSO item;
    [SerializeField] ItemSO item2;
    
    
    public void UpdateUI()
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
        

        itemsInInventory.Add(itemsInTransaction);
        itemsInInventory.Add(playerInventory.items);

        transactionUI.UpdateInventory(itemsInInventory);
        transactionUI.UpdateSoulsCount(GetTotalSoulsInTransaction());
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
                        itemSlot.item = item2;
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

    public void MoveItemBetweenLists(int buttonIndexOrigin, int listIndex, int buttonIndexDestiny=-1, int listDestinyIndex=-1)
    {
        if (listIndex == 0)//De shop a transaction
        {
            if (!CheckFreeSloInList(itemsInTransactionShop))return;
            if (buttonIndexDestiny == -1)
            {
                foreach (ItemSlot itemSlot in itemsInTransactionShop)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = shopInventory.items[buttonIndexOrigin].item;
                        break;
                    }
                }
                foreach (ItemSlot itemSlot in shopInventory.items)
                {
                    if (itemSlot.slotNumber == buttonIndexOrigin)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }else
            {
                foreach (ItemSlot itemSlot in itemsInTransactionShop)
                {
                    if (itemSlot.item == null&&itemSlot.slotNumber==buttonIndexDestiny/2)
                    {
                        itemSlot.item = shopInventory.items[buttonIndexOrigin].item;
                        foreach (ItemSlot _itemSlot in shopInventory.items)
                        {
                            if (_itemSlot.slotNumber == buttonIndexOrigin)
                            {
                                _itemSlot.item = null;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        else if (listIndex == 1)//De transaccion a shop
        {
            if (!shopInventory.CheckFreeSlot())return;
            if (buttonIndexDestiny == -1)//es presionado
            {
                foreach (ItemSlot itemSlot in shopInventory.items)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = itemsInTransactionShop[buttonIndexOrigin/2].item;
                        break;
                    }
                }
                foreach (ItemSlot itemSlot in itemsInTransactionShop)
                {
                    if (itemSlot.slotNumber == buttonIndexOrigin/2)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }else// es arrastrado
            {
                foreach (ItemSlot itemSlot in shopInventory.items)
                {
                    if (itemSlot.item == null&&itemSlot.slotNumber==buttonIndexDestiny&&listDestinyIndex==0)
                    {
                        itemSlot.item = itemsInTransactionShop[buttonIndexOrigin/2].item;
                        foreach (ItemSlot _itemSlot in itemsInTransactionShop)
                        {
                            if (_itemSlot.slotNumber == buttonIndexOrigin/2)
                            {
                                _itemSlot.item = null;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            
        }
        else if (listIndex == 2)//De la transaccion al player
        {
            if (!playerInventory.CheckFreeSlot())return;
            if (buttonIndexDestiny == -1)
            {
                foreach (ItemSlot itemSlot in playerInventory.items)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = itemsInTransactionPlayer[buttonIndexOrigin/2].item;
                        break;
                    }
                }
                foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
                {
                    if (itemSlot.slotNumber == buttonIndexOrigin/2)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }else
            {
                foreach (ItemSlot itemSlot in playerInventory.items)
                {
                    if (itemSlot.item == null&&itemSlot.slotNumber==buttonIndexDestiny&&listDestinyIndex==2)
                    {
                        itemSlot.item = itemsInTransactionPlayer[buttonIndexOrigin/2].item;
                        foreach (ItemSlot _itemSlot in itemsInTransactionPlayer)
                        {
                            if (_itemSlot.slotNumber == buttonIndexOrigin/2)
                            {
                                _itemSlot.item = null;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
        else if (listIndex == 3)//De player a transaccion
        {
            if (!CheckFreeSloInList(itemsInTransactionPlayer))return;
            if (buttonIndexDestiny == -1)
            {
                foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
                {
                    if (itemSlot.item == null)
                    {
                        itemSlot.item = playerInventory.items[buttonIndexOrigin].item;
                        break;
                    }
                }
                foreach (ItemSlot itemSlot in playerInventory.items)
                {
                    if (itemSlot.slotNumber == buttonIndexOrigin)
                    {
                        itemSlot.item = null;
                        break;
                    }
                }
            }else
            {
                foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
                {
                    if (itemSlot.item == null&&itemSlot.slotNumber==buttonIndexDestiny/2)
                    {
                        itemSlot.item = playerInventory.items[buttonIndexOrigin].item;
                        foreach (ItemSlot _itemSlot in playerInventory.items)
                        {
                            if (_itemSlot.slotNumber == buttonIndexOrigin)
                            {
                                _itemSlot.item = null;
                                break;
                            }
                        }
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
    public void CancelTransaction()
    {
        foreach (ItemSlot itemSlot in itemsInTransactionShop)
        {
            if (itemSlot.item != null)
            {
                MoveItemBetweenLists(itemSlot.slotNumber*2, 1);
            }
        }
        foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
        {
            if (itemSlot.item != null)
            {
               MoveItemBetweenLists(itemSlot.slotNumber*2, 2);
            }
        }
        
        gameObject.SetActive(false);
    }
    public int GetTotalSoulsInTransaction()
    {
        int totalSouls = 0;
        foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
        {
            if (itemSlot.item != null)
            {
                totalSouls += (int)(itemSlot.item.price*0.75f);
            }
            
        }
        foreach (ItemSlot itemSlot in itemsInTransactionShop)
        {
            if (itemSlot.item != null)
            {
                totalSouls -= itemSlot.item.price;
            }
            
        }
        return totalSouls;
    
    }

    public void AcceptTansaction()
    {
        if ((GetTotalSoulsInTransaction()*-1)>playerInventory.soulsCount)
        {
            Debug.LogWarning("Not enough souls");
            return;
        }else if (shopInventory == null)
        {
            Debug.LogWarning("No shop selected");
        }
        else
        {
            foreach (ItemSlot itemSlot in itemsInTransactionPlayer)
            {
                if (itemSlot.item != null)
                {
                    shopInventory.AddItem(itemSlot.item);

                }
            }
            
            foreach (ItemSlot itemSlot in itemsInTransactionShop)
            {
                if (itemSlot.item != null)
                {
                    playerInventory.AddItem(itemSlot.item);
                    
                }
            }
            // playerInventory.soulsCount+=GetTotalSoulsInTransaction();
            playerInventory.AddSouls(GetTotalSoulsInTransaction());
            if (shopInventory != null)
            {
                shopInventory.AddSouls(GetTotalSoulsInTransaction()*-1);
            }
            itemsInTransactionPlayer.Clear();
            itemsInTransactionShop.Clear();
            OnTransactionEnds?.Invoke();
            if (shopInventory.TryGetComponent<ShopInteraction>(out ShopInteraction shopInteraction))
            {
                shopInteraction.CloseInteraction();
            }
        }
    }

}
