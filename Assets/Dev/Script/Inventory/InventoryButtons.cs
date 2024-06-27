using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButtons : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{

    public static event OnItemDropped OnItemDragged;
    public static event OnItemDropped OnItemDraggedStarts;
    public delegate void OnItemDropped(GameObject slotButton);
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
              
                transactionUI.MoveItemOnClick(item, slotButton);
            }


            if (playerInventoryUIManager != null)
            {
                playerInventoryUIManager.UseItem(slotButton);
            }
            
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnItemDraggedStarts?.Invoke(this.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnDrop(PointerEventData eventData)
    {
        
        OnItemDragged?.Invoke(this.gameObject);

    }
}
