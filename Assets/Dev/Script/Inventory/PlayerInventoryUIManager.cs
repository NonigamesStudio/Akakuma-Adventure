using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInventoryUIManager : MonoBehaviour
{

    
   
    List <Button> buttons; 
    [SerializeField] GameObject panel;
    [SerializeField] GameObject playerInventoryUI;
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject player;
    public bool isUIOpen=false;
    void OnEnable()
    {

        if (buttons == null)
        {
            InitializeButtons();
        }   
        UpdateInventory();
        inventory.OnItemListChange += UpdateInventory;
        
    }
    void OnDisable()
    {
        inventory.OnItemListChange -= UpdateInventory;
    }

    void Start()
    {
        InitializeButtons();
        UpdateInventory();
    }

    private void InitializeButtons()
    {
        buttons = new List<Button>();
        Transform parent = panel.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            Button button = child.GetComponent<Button>();
            buttons.Add(button);
            if (button.TryGetComponent(out InventoryButtons inventoryButtons))
            {
                inventoryButtons.UpdateImage(null);
                inventoryButtons.slotImage.enabled = false;
            }
        }
    }


    public void UpdateInventory()
    {
        
        foreach (Button button in buttons)
        {
            if (button.TryGetComponent(out InventoryButtons inventoryButtons))
            {
                inventoryButtons.UpdateImage(null);
                inventoryButtons.UpdateItem(null);
                inventoryButtons.slotImage.enabled = false;
            }
        }
        List<ItemSO> items = inventory.items;
        for (int i = 0; i < items.Count; i++)
        {
            if (buttons[i].TryGetComponent(out InventoryButtons inventoryButtons))
            {
                inventoryButtons.UpdateImage(items[i].itemSprite);
                inventoryButtons.UpdateItem(items[i]);
                inventoryButtons.slotImage.enabled = true;
            }
        }
    }

    public void UseItem(ItemSO item)
    {
        item.Use(player);
    }
    

}
