using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInventoryUIManager : MonoBehaviour
{
    List <Button> buttons; 
    [SerializeField] GameObject panel;
    [SerializeField] GameObject playerInventoryUI;
    [SerializeField] GameObject quickAcessInventory;
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject player;
    int slotOriginIndex;
    public bool isUIOpen=false;
    void OnEnable()
    {

        if (buttons == null)
        {
            InitializeButtons();
        }   
        UpdateInventory();
        inventory.OnItemListChange += UpdateInventory;
        InventoryButtons.OnItemDragged += ItemDragged;
        InventoryButtons.OnItemDraggedStarts += OnItemDraggedStarts;

        
    }
     void OnDisable()
    {
        inventory.OnItemListChange -= UpdateInventory;
        InventoryButtons.OnItemDragged -= ItemDragged;
        InventoryButtons.OnItemDraggedStarts -= OnItemDraggedStarts;
    }

    private void OnItemDraggedStarts(GameObject slotButton)
    {
        if (slotButton.TryGetComponent(out Button inventoryButtons))
        {
            foreach (Button button in buttons)
            {
                if (button == inventoryButtons)
                {
                    slotOriginIndex=buttons.IndexOf(button);
                }
            }
        }
    }
    private void  ItemDragged(GameObject slotButton)
    {
        int slotIndexFinal=-1;
        if (slotButton.TryGetComponent(out Button inventoryButtons))
        {
            foreach (Button button in buttons)
            {
                if (button == inventoryButtons)
                {
                    slotIndexFinal=buttons.IndexOf(button);
                }
            }
        }
        if (slotIndexFinal != -1 && slotIndexFinal != slotOriginIndex) 
        {
            inventory.SwapItems(slotOriginIndex, slotIndexFinal);

        }else if (slotIndexFinal == -1)
        {
            //Debug.Log("No se encontró el botón en la lista de botones.");
        }else if (slotIndexFinal == slotOriginIndex)
        {
            //Debug.Log("Item Dragged to the same slot");
        }
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
        for (int i = 0; i < quickAcessInventory.transform.childCount; i++)
        {
            Transform child = quickAcessInventory.transform.GetChild(i);
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
