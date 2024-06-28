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
    bool isBeingDragged = false;
    [SerializeField] private PlayerInventoryUIManager playerInventoryUIManager;
    [SerializeField] private TransactionUI transactionUI;
    [SerializeField] private Canvas gameCanvas;

   

    void Start ()
    {  
        slotButton.onClick.AddListener(ButtonClicked);
        Canvas canvas = transform.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
           gameCanvas=canvas;
        }
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
        if (isBeingDragged)return;
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
        isBeingDragged = true;
        OnItemDraggedStarts?.Invoke(this.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (gameCanvas != null)
        {
            slotImage.transform.SetParent(gameCanvas.transform);
        }
        slotImage.raycastTarget = false;
        slotImage.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        slotImage.transform.SetParent(this.transform);
        isBeingDragged = false;
        slotImage.raycastTarget = true;
        slotImage.transform.position=slotButton.transform.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        
        OnItemDragged?.Invoke(this.gameObject);

    }
}
