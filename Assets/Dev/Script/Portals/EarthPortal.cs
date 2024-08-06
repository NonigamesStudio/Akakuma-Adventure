using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EarthPortal : Portal
{
    [SerializeField] Animator animCutOff;
    [SerializeField] WarningMessage warningMessage;
    Player player;
    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {   
            if (player.TryGetComponent<Inventory>(out Inventory inventory))
            {
                foreach(ItemSlot itemSlot in inventory.items)
                {
                    if (itemSlot.item==null) continue;
                    if (itemSlot.item is PortalTicket ticket)
                    {
                        if (ticket.validForPortalType==PortalType.Earth)
                        {
                            warningMessage.gameObject.SetActive(true);
                            warningMessage.acceptButton.onClick.AddListener(()=>{Teleport(other.gameObject);});
                            warningMessage.SetText("Traveling to Earth Island will consume " + ticket.itemName + " do you want to proceed?");
                        }
                    }  
                }
            }
        }
    }

    public override void Teleport(GameObject objectToTeleport)
    {
        warningMessage.gameObject.SetActive(false);
        
        
        if (objectToTeleport.TryGetComponent<Player>(out Player player))
        {
            if (player.TryGetComponent<Inventory>(out Inventory inventory))
            {
                foreach(ItemSlot itemSlot in inventory.items)
                {
                    if (itemSlot.item is PortalTicket ticket)
                    {
                        if (ticket.validForPortalType==PortalType.Earth)
                        {
                            PortalTicket portalTicket = itemSlot.item as PortalTicket;
                            portalTicket.canBeUsedFromInventory = true;
                            inventory.UseItem(itemSlot.slotNumber);
                        }
                    }  
                
                }
            }
            this.player=player;
            player.GetStuned(2f);
            GameManager.instance.currentPlayerData = new DataToTransfer(player);
        }
        animCutOff.Play("RTransitionImgAnim");
        LeanTween.delayedCall(2f, () => { 
        ChangeScenes(GameManager.scenes.DevIsla1, new List<GameManager.scenes> {
        GameManager.scenes.DevIsla1,GameManager.scenes.ArtIsla1}/*, new List<GameManager.scenes> {
        GameManager.scenes.MainMenu}*/);
        });
        
    }
    public override void ChangeScenes(GameManager.scenes sceneToSetActive, List<GameManager.scenes> scenesLoad, List<GameManager.scenes> scenesUnload=null)
    {
        GameManager.instance.ChangeScenes(sceneToSetActive, scenesLoad, scenesUnload);
    }
}
