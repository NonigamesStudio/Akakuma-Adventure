using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopInteraction :MonoBehaviour, Interactable
{
    [SerializeField] TransactionManager transactionManager;
    private bool isTransactionOpen;
    [SerializeField] Player player;
    public void Start()
    {
    player.OnSCPPress += CloseInteraction;
    }
    public void Interact()
    {
        Debug.Log("Interaction");
        transactionManager.gameObject.SetActive(true);
        isTransactionOpen=true;
    }
    public void CloseInteraction()
    {
        if (isTransactionOpen)
        {
       transactionManager.gameObject.SetActive(false);
        isTransactionOpen=false;
        }
        
    }





}
