using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopInteraction :MonoBehaviour, Interactable
{
    public event System.Action OnCloseInteraction;
    [SerializeField] TransactionManager transactionManager;
    [SerializeField] Inventory inventory;
    private bool isTransactionOpen;
    [SerializeField] Player player;
    public InteractableType interactableType {get =>InteractableType.Shop; }

    public void Start()
    {
        player.OnSCPPress += CloseInteraction;
    }
    public void Interact()
    {
        transactionManager.gameObject.SetActive(true);
        transactionManager.InitializeTransaction(inventory);
        isTransactionOpen=true;
    }
    public void CloseInteraction()
    {
        if (isTransactionOpen)
        {
        transactionManager.gameObject.SetActive(false);
        isTransactionOpen=false;
        OnCloseInteraction?.Invoke();
        }
        
    }





}
