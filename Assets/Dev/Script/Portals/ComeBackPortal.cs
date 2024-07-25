using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeBackPortal : Portal
{
    [SerializeField] Animator animCutOff;
    
    public override void ChangeScenes(GameManager.scenes sceneToSetActive, List<GameManager.scenes> scenesLoad, List<GameManager.scenes> scenesUnload=null)
    {
        GameManager.instance.ChangeScenes(sceneToSetActive, scenesLoad, scenesUnload);
    }

    public override void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.TryGetComponent<Player>(out Player player))
        {  
            animCutOff.Play("RTransitionImgAnim");
            player.GetStuned(2f);
            GameManager.instance.currentPlayerData = new DataToTransfer(player);
            LeanTween.delayedCall(2f, () => {
            ChangeScenes(GameManager.scenes.DevIsla2, new List<GameManager.scenes> {
            GameManager.scenes.DevIsla2,GameManager.scenes.ArtIsla2}/*, new List<GameManager.scenes> {
            GameManager.scenes.MainMenu}*/);
        });
            
        }
    }

    public override void Teleport(GameObject objectToTeleport)
    {
        throw new System.NotImplementedException();
    }
}
