using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopInteraction :MonoBehaviour, Interactable
{
    public event System.Action OnOpenInteraction;
    public event System.Action OnCloseInteraction;
    [SerializeField] TransactionManager transactionManager;
    [SerializeField] Inventory inventory;
    private bool isTransactionOpen;
    [SerializeField] Player player;
    PlayerInventory playerInventory;
    public InteractableType interactableType {get =>InteractableType.Shop; }

    public void OnEnable()
    {
        player.OnSCPPress += CloseInteraction;

        if (player.TryGetComponent<PlayerInventory>(out PlayerInventory playerInventory))    
        {
            this.playerInventory=playerInventory;   
        }
        OnOpenInteraction += playerInventory.HideQuickAccessInventory;
        OnCloseInteraction += playerInventory.ShowQuickAccessInventory;
    }
    public void OnDisable()
    {
        player.OnSCPPress -= CloseInteraction;
        OnOpenInteraction -= playerInventory.HideQuickAccessInventory;
        OnCloseInteraction -= playerInventory.ShowQuickAccessInventory;
        
    }
    public void Interact()
    {
        transactionManager.gameObject.SetActive(true);
        transactionManager.InitializeTransaction(inventory);
        OnOpenInteraction?.Invoke();
        isTransactionOpen=true;
    }
    public void CloseInteraction()
    {
        if (isTransactionOpen)
        {
        //transactionManager.gameObject.SetActive(false);
        transactionManager.CancelTransaction();
        isTransactionOpen=false;
        StartCoroutine(CallCloseInteraction(0.1f));

        }
        
    }

    private IEnumerator CallCloseInteraction(float sec)
    {
        yield return new WaitForSeconds(sec);
        OnCloseInteraction?.Invoke();   
        
    }





}
