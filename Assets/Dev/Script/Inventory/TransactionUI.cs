using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransactionUI : MonoBehaviour
{
    [SerializeField]List<List<Button>> buttons = new List<List<Button>>(); //dimension 1: shop, 2: transaction, 3: player
    [SerializeField] GameObject[] panels = new GameObject[3];
    [SerializeField] TransactionManager transactionManager;
    [SerializeField] Button closeButton;
    [SerializeField] Button acceptButton;
    [SerializeField] TMP_Text soulsCount;
    Button slotOriginIndex;
    
    
    void Awake()
    {
        InitializeTransaction();
    }

    void OnEnable()
    {
        InventoryButtons.OnItemDragged += ItemDragged;
        InventoryButtons.OnItemDraggedStarts += ItemDraggedStarts;
        if (transactionManager.playerInventory.gameObject.TryGetComponent<Player>(out Player player))
        {   
            closeButton.onClick.AddListener(player.ClosUIInteraction);
        }
        
    }
    void OnDisable()
    {
        InventoryButtons.OnItemDragged -= ItemDragged;
        InventoryButtons.OnItemDraggedStarts -= ItemDraggedStarts;
        if (transactionManager.playerInventory.gameObject.TryGetComponent<Player>(out Player player))
        {   
            closeButton.onClick.RemoveListener(player.ClosUIInteraction);
        }
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

    public void MoveItemOnClick(ItemSO item, Button button)
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

        MoveObject(listIndex, buttonIndex);
    }

    public void ItemDraggedStarts(GameObject slotButton)
    {
        if (slotButton.TryGetComponent<Button>( out Button button))
        {
            slotOriginIndex = button;
        }
    }
    public void ItemDragged(GameObject slotButton)
    {
        int originalListIndex=-1;
        int originalButtonIndex=-1;
        int destinyListIndex=-1;
        int destinyButtonIndex=-1;

        for (int i = 0; i < buttons.Count; i++)
        {
            for (int j = 0; j < buttons[i].Count; j++)
            {
                if (slotOriginIndex == buttons[i][j])
                {
                    originalListIndex=i;
                    originalButtonIndex=j;
                    break;
                }
            }

        }
        if (slotButton.TryGetComponent<Button>(out Button button))
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                for (int j = 0; j < buttons[i].Count; j++)
                {
                    if (button == buttons[i][j])
                    {
                        destinyListIndex=i;
                        destinyButtonIndex=j;
                        break;
                    }
                }
            }
        }
        if(destinyListIndex==-1||originalListIndex==-1||destinyButtonIndex==-1||originalButtonIndex==-1)return;

        if(originalListIndex==0&&destinyListIndex==1)//De shop a transaction
        {
            MoveObject(0,originalButtonIndex,destinyButtonIndex);
        }
        if((originalListIndex==1&&destinyListIndex==0)||(originalListIndex==1&&destinyListIndex==2))//De transaccion a inventario
        {
            MoveObject(1,originalButtonIndex,destinyButtonIndex,destinyListIndex);
        }
        if(originalListIndex==2&&destinyListIndex==1)//De player a transaction
        {
            MoveObject(2,originalButtonIndex,destinyButtonIndex);
        }
        if ((originalListIndex ==  destinyListIndex)&&destinyListIndex!=1)//dentro del mismo inventario
        {
            if (originalListIndex==0)
            {
                transactionManager.shopInventory.SwapItems(originalButtonIndex, destinyButtonIndex);
            }else if (originalListIndex==2)
            {
                transactionManager.playerInventory.SwapItems(originalButtonIndex, destinyButtonIndex);
            }
            transactionManager.UpdateUI();
        }

    }
    
    private void MoveObject(int listIndex, int buttonIndexOrigin, int buttonIndexDestiny = -1, int listDestinyIndex=-1)
    {
        if (listIndex == 0)
        {
            if (buttonIndexDestiny % 2 == 0||buttonIndexDestiny==-1)
            {
            transactionManager.MoveItemBetweenLists(buttonIndexOrigin, 0, buttonIndexDestiny);
            }
        }
        else if (listIndex == 1)
        {
            if (buttonIndexOrigin % 2 == 0)//De transaccion a shop
            {
                transactionManager.MoveItemBetweenLists(buttonIndexOrigin, 1, buttonIndexDestiny, listDestinyIndex);
            }
            else//De transaccion a player
            {
                transactionManager.MoveItemBetweenLists(buttonIndexOrigin, 2, buttonIndexDestiny, listDestinyIndex);
            }
        }
        else if (listIndex == 2)
        {
            if (buttonIndexDestiny % 2 == 1||buttonIndexDestiny==-1)
            {
                transactionManager.MoveItemBetweenLists(buttonIndexOrigin, 3, buttonIndexDestiny);
            }
            
        }
        
    }
    public void UpdateSoulsCount(int souls=0)
    {
        soulsCount.text = souls.ToString();
    }
}