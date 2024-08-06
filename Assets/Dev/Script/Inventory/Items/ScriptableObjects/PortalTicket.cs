using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/PortalTicket Fire")]
public class PortalTicket : ItemSO
{
    public PortalType validForPortalType;

    void OnEnable()
    {
        canBeUsedFromInventory=false;
    }

    public override void Use(GameObject user=null)
    {
        
    }
}

public enum PortalType
{
    Fire,
    Water,
    Earth,
}
