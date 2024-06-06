using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransactionManager : MonoBehaviour
{
    public Inventory shopInventory=null;
    public Inventory playerInventory;
    [SerializeField] TransactionUI transactionUI;
    List<ItemSO> itemsInTransactionPlayer = new List<ItemSO>(); 
    List<ItemSO> itemsInTransactionShop = new List<ItemSO>();
    [SerializeField] ItemSO item;
    
    void UpdateUI()
    {
        List<List<ItemSO>> itemsInInventory = new List<List<ItemSO>>();
        List<ItemSO> itemsInTransaction = new List<ItemSO>(); 

        for (int i = 0; i < itemsInTransactionShop.Count + itemsInTransactionPlayer.Count; i++) //Agrega los elementos de itemsInTransactionShop en los indices pares y los de itemsInTransactionPlayer en los impares
        {
            if (i % 2 == 0 && itemsInTransactionShop.Count > i / 2) 
            {
                itemsInTransaction.Add(itemsInTransactionShop[i / 2]);
            }
            else if (i % 2 == 1 && itemsInTransactionPlayer.Count > i / 2) 
            {
                itemsInTransaction.Add(itemsInTransactionPlayer[i / 2]);
            }
        }
        itemsInInventory.Add(itemsInTransaction);
        itemsInInventory.Add(playerInventory.items);
        //itemsInInventory.Add(shopInventory.items);
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
            itemsInTransactionShop.Add(item);
            UpdateUI();
        }
    }

}
