using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/HealthPotion")]
public class HealthPotion : ItemSO
{
    
    
    public override void Use(GameObject user=null)
    {
        Health health = null;
        if (user.TryGetComponent<Health>(out health))
        {
            health.TakeHealth(30);
            inventory.RemoveItem(this);
        }
       
        
    }
}

