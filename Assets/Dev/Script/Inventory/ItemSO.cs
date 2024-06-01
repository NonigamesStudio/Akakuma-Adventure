using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
   
    [SerializeField] int price = 0;
    [SerializeField] string itemName = "";
    [SerializeField] Sprite itemSprite = null;


}
