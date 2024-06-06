using System;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    
    public int price = 0;
    public string itemName = "";
    public Sprite itemSprite = null;
    public GameObject itemPrefab = null;
    public Inventory inventory;
   
    public virtual void Use(object parameters=null)
    {
        
    }
}
