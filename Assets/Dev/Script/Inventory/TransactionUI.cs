using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionUI : MonoBehaviour
{
    [SerializeField]List<List<Button>> buttons = new List<List<Button>>(); //dimension 1: shop, 2: transaction, 3: player
    [SerializeField] GameObject[] panels = new GameObject[3];
    [SerializeField] TransactionManager transactionManager;
    
    
    void Awake()
    {
        InitializeTransaction();
    }


    void InitializeTransaction()
    {
        //dimension 1: shop, 2: transaction, 3: player
        for (int i = 0; i < panels.Length; i++)
        {
            
            Transform parent = panels[i].transform;
            List<Button> panelButtonList = new List<Button>();
            buttons.Add(panelButtonList);

            for (int j = 0; j < parent.childCount; j++)
            {
                Transform child = parent.GetChild(j);
                Button button = child.GetComponent<Button>();
                panelButtonList.Add(button);
                if (button.TryGetComponent(out InventoryButtons inventoryButtons))
                {
                    inventoryButtons.UpdateImage(null);
                    inventoryButtons.slotImage.enabled = false;
                }
            }
        }
    }   
    public void UpdateInventory(List<List<ItemSlot>> itemsInTransaction)
    {
        
        if (itemsInTransaction == null)
        {
            Debug.LogWarning("itemsInTransaction is null");
            return;
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            for (int j = 0; j < buttons[i].Count; j++)
            {
                if (buttons[i][j] != null && buttons[i][j].TryGetComponent(out InventoryButtons inventoryButtons))
                {
                    inventoryButtons.UpdateImage(null);
                    inventoryButtons.UpdateItem(null);
                    inventoryButtons.slotImage.enabled = false;
                }
            }
        }

        for (int i = 0; i < itemsInTransaction.Count; i++)
        {
            
            if (itemsInTransaction[i] == null || i >= buttons.Count) continue;

            for (int j = 0; j < itemsInTransaction[i].Count; j++)
            {
                
                if (j >= buttons[i].Count) continue;

                if (buttons[i][j] != null && buttons[i][j].TryGetComponent(out InventoryButtons inventoryButtons))
                {
                    if (itemsInTransaction[i][j] != null && itemsInTransaction[i][j].item != null)
                    {
                        inventoryButtons.UpdateImage(itemsInTransaction[i][j].item.itemSprite);
                        inventoryButtons.UpdateItem(itemsInTransaction[i][j].item);
                        inventoryButtons.slotImage.enabled = true;
                    }
                }
            }
        }
    }

    public void MoveItem(ItemSO item, Button button)
    {
        
        int listIndex = 0; //0: shop, 1 y 2: transaction, 3: player
        int buttonIndex = 0;
    
        for (int i = 0; i < buttons.Count; i++)
        {
            for (int j = 0; j < buttons[i].Count; j++)
            {
                if (buttons[i][j] == button)
                {
                    listIndex = i;
                    buttonIndex = j;
                    break;
                }
            }
    
        }
    
        if (listIndex == 0)
        {
            transactionManager.MoveItem(buttonIndex, 0);
        }
        else if (listIndex == 1)
        {
            if (buttonIndex % 2 == 0)
            {
                transactionManager.MoveItem(buttonIndex, 1);
            }
            else
            {
                transactionManager.MoveItem(buttonIndex, 2);
            }
        }
        else if (listIndex == 2)
        {
            transactionManager.MoveItem(buttonIndex, 3);
        }
    }
    
}
