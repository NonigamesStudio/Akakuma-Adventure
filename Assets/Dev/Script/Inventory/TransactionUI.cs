using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransactionUI : MonoBehaviour
{
    [SerializeField]List<List<Button>> buttons = new List<List<Button>>(); //dimension 1: shop, 2: transaction, 3: player
    [SerializeField] GameObject[] panels = new GameObject[3];
    [SerializeField] TransactionManager transactionManager;
    
    
    void Start()
    {
        InitializeTransaction();
    }


    void InitializeTransaction()
    {
        //dimension 1: shop, 2: transaction, 3: player
        for (int i = 0; i < panels.Length; i++)
        {
            Debug.Log("Panel: " + panels[i].name);
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
    public void UpdateInventory(List<List<ItemSO>> itemsInTransaction)
    {
        for (int i = 0; i < itemsInTransaction.Count; i++)
        {
            if (itemsInTransaction[i] == null) continue;
            for (int j = 0; j < itemsInTransaction[i].Count; j++)
            {
                if (itemsInTransaction[i][j] == null) continue;
                if (buttons[i][j].TryGetComponent(out InventoryButtons inventoryButtons))
                {
                   
                    inventoryButtons.UpdateImage(itemsInTransaction[i][j].itemSprite);
                    inventoryButtons.UpdateItem(itemsInTransaction[i][j]);
                    inventoryButtons.slotImage.enabled = true;
                    
                    
                }
            }
        }
    }
}
