using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2BossState : StateBoss
{
    [SerializeField] AIBossEnemy[] enemiesPool;
    [SerializeField] List<Transform> refsPos;
    [SerializeField] Transform playerT;
    [SerializeField] Animator anim;

    private void OnEnable()
    {
        anim.SetTrigger("Invoque");

        LeanTween.delayedCall(0.7f, () =>
        {

            List<Transform> temp = new List<Transform>();
            for (int i = 0; i < 2; i++)
            {
                foreach (AIBossEnemy item in enemiesPool)
                {
                    if (!item.gameObject.activeSelf)
                    {
                        Transform refPos = refsPos[Random.Range(0, refsPos.Count)];
                        item.transform.position = new Vector3(refPos.position.x, 1.5f, refPos.position.z);
                        item.gameObject.SetActive(true);
                        temp.Add(refPos);
                        refsPos.Remove(refPos);
                        break;
                    }
                }
            }

            foreach (Transform item in temp)
            {
                refsPos.Add(item);
            }
        });
        StartCoroutine(ColdDown());
    }

    IEnumerator ColdDown()
    {
        yield return new WaitForSeconds(2);
        bossStateMachine.ChangeToWalkState();
        OnColdDown = true;
        yield return new WaitForSeconds(coolDown);
        OnColdDown = false;
    }
}
