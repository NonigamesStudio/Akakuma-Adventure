using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] ItemSO item;
    // Start is called before the first frame update
    void Start()
    {
        inventorySize=20;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItem(item);
            
            
        }
    }

}
