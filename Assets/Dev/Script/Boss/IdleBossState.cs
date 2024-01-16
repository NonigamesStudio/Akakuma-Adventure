using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBossState : StateBoss
{
    [SerializeField] Transform playerT;

    private void OnEnable()
    {
        StartCoroutine(CoolDownToChangeState());
    }
    private void Update()
    {
        Vector3 dir = playerT.position - transform.position;
        transform.forward = dir.normalized;
    }

    IEnumerator CoolDownToChangeState()
    {
        yield return new WaitForSeconds(2);
        bossStateMachine.RandomState();
    }
}
