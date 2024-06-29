using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/HealthPotion")]
public class HealthPotion : ItemSO
{   
    [SerializeField] int healthCurationAmt = 10;
    public override void Use(GameObject user=null)
    {
        Health health = null;
        
        if (user.TryGetComponent<Health>(out health))
        {
            
            health.TakeHealth(healthCurationAmt);
            Debug.Log("used");
        }
    }
}

