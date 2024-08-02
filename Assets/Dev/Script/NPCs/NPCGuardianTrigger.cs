using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGuardianTrigger : MonoBehaviour
{
   [SerializeField]DialogueTriggerInfo dialogueTriggerInfo;
   
   void OnTriggerEnter(Collider other)
   {
        if (dialogueTriggerInfo.playedAlready) return;
        dialogueTriggerInfo.Interact();
        dialogueTriggerInfo.playedAlready=true;
   }
}
