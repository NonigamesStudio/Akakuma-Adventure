using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1BossState : StateBoss
{
    
    [SerializeField] Transform[] pointsRefsToShoot;
    [SerializeField] Projectil prefabPorjectil;
    [SerializeField] float dmg;

    
    private void OnEnable()
    {
        StartCoroutine(ColdDown());
        
    }

    void Shoot3Projectile()
    {
        foreach (Transform refShoot in pointsRefsToShoot)
        {
            Projectil clonPrefabPorjectil = Instantiate(prefabPorjectil, refShoot.transform.position, refShoot.transform.rotation);
            clonPrefabPorjectil.dmg = dmg;
            clonPrefabPorjectil.Shoot();
        }

        
    }

    IEnumerator ColdDown()
    {
        yield return new WaitForSeconds(2);
        Shoot3Projectile();
        yield return new WaitForSeconds(2);
        bossStateMachine.ChangeToWalkState();
        OnColdDown = true;
        yield return new WaitForSeconds(coolDown);
        OnColdDown = false;
    }
}
