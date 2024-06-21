using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{

    public event Action OnClick;
    public Button slotButton;
    public Image slotImage;
    public ItemSO item;
    public GameObject player;
    [SerializeField] private PlayerInventoryUIManager playerInventoryUIManager;
    [SerializeField] private TransactionUI transactionUI;

   

    void Start ()
    {  
        slotButton.onClick.AddListener(ButtonClicked);
    }

    public void UpdateImage(Sprite sprite)
    {
        slotImage.sprite = sprite;
    }

    public void UpdateItem(ItemSO item)
    {
        this.item=item;
    }

    private void ButtonClicked()
    {
        if (item != null)
        {
            if (transactionUI != null)
            {
                transactionUI.MoveItem(item, slotButton);
            }


            if (playerInventoryUIManager != null)
            {
                playerInventoryUIManager.UseItem(item);
            }
            
        }
    }
    

}
