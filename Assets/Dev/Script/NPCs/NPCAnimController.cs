using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimController : MonoBehaviour
{
    public Action onWorkAnimationEnds;
    [SerializeField] Animator anim;
    [SerializeField] UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] Transform parentTransform;

    public void SetAnimation (NPCAnimState npcState)
    {
        switch (npcState)
        {
            case NPCAnimState.Walking:
                anim.SetBool("Walking", true);
                anim.SetBool("Sit", false);
                anim.SetBool("TurningLeft", false);
                anim.SetBool("TurningRight", false);
                break;
            case NPCAnimState.Idle:
                anim.SetBool("Walking", false);
                anim.SetBool("Sit", false);
                anim.SetBool("TurningLeft", false);
                anim.SetBool("TurningRight", false);
                break;
            case NPCAnimState.Working:
                anim.SetBool("Walking", false);
                anim.SetBool("Sit", false);
                anim.SetTrigger("Working");
                anim.SetBool("TurningLeft", false);
                anim.SetBool("TurningRight", false);
                break;
            case NPCAnimState.Sit:
                anim.SetBool("Walking", false);
                anim.SetBool("Sit", true);
                break;
            case NPCAnimState.RotatingRight:
                anim.SetBool("Walking", false);
                anim.SetBool("Sit", false);
                anim.SetBool("TurningLeft", false);
                anim.SetBool("TurningRight", true);
                break;
            case NPCAnimState.RotatingLeft:
                anim.SetBool("Walking", false);
                anim.SetBool("Sit", false);
                anim.SetBool("TurningLeft", true);
                anim.SetBool("TurningRight", false);
                break;
           
        }
    }
    public void WorkEnds()
    {
        //Se llama desde un evento de la animacion
        onWorkAnimationEnds?.Invoke();
       
    }
    public void SetLayerWeight(int layerIndex, float weight)
    {
        
        anim.SetLayerWeight(layerIndex, weight);
        
    }
}

public enum NPCAnimState
{
    Walking,
    Idle,
    Working,
    Sit,
    RotatingRight,
    RotatingLeft,

}


