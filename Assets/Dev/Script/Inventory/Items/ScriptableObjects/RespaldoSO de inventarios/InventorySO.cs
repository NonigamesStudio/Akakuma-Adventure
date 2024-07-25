using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory/Inventory")]
public class InventorySO : ScriptableObject
{
    public int inventorySize = 20;
    public int soulsCount;
    public ItemSlot[] items;

    public void CopyItemsFromInventory(Inventory inventory)
    { 
       
        if (inventorySize!=inventory.inventorySize)
        {
            inventorySize = inventory.inventorySize;
        }
       
        soulsCount = inventory.soulsCount;

        if (items == null || items.Length != inventorySize)
        {
            items = new ItemSlot[inventorySize];
        }

       
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory.items[i] != null)
            {
                items[i] = new ItemSlot(inventory.items[i].item, inventory.items[i].slotNumber);
            }
            else
            {
                items[i] = new ItemSlot(null, i);
            }
        }
    }

    public void CopyItemsFromSO(InventorySO inventory)
    { 
       
        if (inventorySize!=inventory.inventorySize)
        {
            inventorySize = inventory.inventorySize;
        }
       
        soulsCount = inventory.soulsCount;

        if (items == null || items.Length != inventorySize)
        {
            items = new ItemSlot[inventorySize];
        }

       
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory.items[i] != null)
            {
                items[i] = new ItemSlot(inventory.items[i].item, inventory.items[i].slotNumber);
            }
            else
            {
                items[i] = new ItemSlot(null, i);
            }
        }
    }
}