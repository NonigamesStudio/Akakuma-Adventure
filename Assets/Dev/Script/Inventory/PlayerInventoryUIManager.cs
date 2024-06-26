using System.Collections.Generic;
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

    public void InitializeButtons()
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
        List<ItemSlot> items = inventory.items;
        for (int i = 0; i < items.Count; i++)
        {
            if (buttons[i].TryGetComponent(out InventoryButtons inventoryButtons))
            {
                if (items[i].item != null) 
                {
                    inventoryButtons.UpdateImage(items[i].item.itemSprite);
                    inventoryButtons.UpdateItem(items[i].item);
                    inventoryButtons.slotImage.enabled = true;
                }else
                {
                    inventoryButtons.UpdateImage(null);
                    inventoryButtons.UpdateItem(null);
                    inventoryButtons.slotImage.enabled = false;
                }
            }
        }
    }

    public void UseItem(Button slot=null)
    {
        if (slot == null)
        {
            Debug.LogError("No se proporcionó un botón.");
            return;
        }

        int index = buttons.IndexOf(slot);

        if (index == -1)
        {
            Debug.LogError("El botón no se encuentra en la lista de botones.");
        }
        else
        {
            inventory.UseItem(index);
        }
    }
}
