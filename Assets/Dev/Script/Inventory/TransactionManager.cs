using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransactionManager : MonoBehaviour
{
    public Inventory shopInventory=null;
    public Inventory playerInventory;
    [SerializeField] TransactionUI transactionUI;
    [SerializeField]List<ItemSO> itemsInTransactionPlayer = new List<ItemSO>(); 
    [SerializeField]List<ItemSO> itemsInTransactionShop = new List<ItemSO>();
    [SerializeField] ItemSO item;
    [SerializeField] ItemSO item2;
    
    void UpdateUI()
    {
        List<List<ItemSO>> itemsInInventory = new List<List<ItemSO>>();

        #region create a list with elements in itemsInTransactionPlayer and itemsInTransactionShop
        
        int totalSize = 2 * System.Math.Max(itemsInTransactionPlayer.Count, itemsInTransactionShop.Count);

        List<ItemSO> itemsInTransaction = new List<ItemSO>(new ItemSO[totalSize]);

        for (int i = 0; i < itemsInTransactionShop.Count; i++)
        {
            
            itemsInTransaction[(i*2)+1] =itemsInTransactionShop[i];
             
        }
        for (int i = 0; i < itemsInTransactionPlayer.Count; i++)
        {
            itemsInTransaction[i*2] =itemsInTransactionPlayer[i];
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
            itemsInTransactionPlayer.Add(item);
            UpdateUI();
        }
         if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemsInTransactionShop.Add(item2);
            UpdateUI();
        }
    }

    public void InitializeTransaction(Inventory shopInventory)
    {
        this.shopInventory = shopInventory;
        UpdateUI();
    }
    

}
