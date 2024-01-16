using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkBossState : StateBoss
{
    
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] Animator anim;

    private void OnEnable()
    {
        agent.isStopped = false;
        StartCoroutine(WalkAction());
        anim.SetBool("walk", true);


    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.position;
    }
    

    IEnumerator WalkAction()
    {
        yield return new WaitForSeconds(coolDown);
        agent.isStopped = true;
        anim.SetBool("walk", false);
        bossStateMachine.RandomState(); 
    }

}
