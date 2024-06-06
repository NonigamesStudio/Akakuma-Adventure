using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{

    public Button slotButton;
    public Image slotImage;
    public ItemSO item;
    public GameObject player;
   

    void Start ()
    {   
        slotButton.onClick.AddListener(UseItem);
    }

    public void UpdateImage(Sprite sprite)
    {
        slotImage.sprite = sprite;
    }

    public void UpdateItem(ItemSO item)
    {
        this.item=item;
    }

    private void UseItem()
    {
        if (item == null)
        {
            Debug.Log("No item ");
            return;
        }
        Debug.Log("Usando " + item.itemName);
        item.Use();
    }


}
