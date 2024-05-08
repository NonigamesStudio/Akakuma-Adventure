using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour, IWeapon
{
    public Player player;

    [Header("Normal Attack Refs")]
    [SerializeField] float damage;
    
    [SerializeField] List <BoxCollider> colliderAttack;
    [SerializeField] float attackDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDown;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject PacticleSlash;
    [SerializeField] bool isEnemy;



    bool isOnCoolDownNormalAttack;
    public void Attack(float bonusDmg)
    {
        if (!isOnCoolDownNormalAttack)
        {
            if (!isEnemy) AnimController_Player.ins.PlayAnim(AnimNamesPlayer.AttackScythe);
            StartCoroutine(AttackAction(bonusDmg));
            StartCoroutine(SkillAnim());
            if (!isEnemy) player.OnWeaponAttack?.Invoke();
            isOnCoolDownNormalAttack = true;
            LeanTween.delayedCall(coolDown, () => { isOnCoolDownNormalAttack = false; });
            
        }
        else
        { //on cooldown
        }
    }

    IEnumerator AttackAction(float bonusdmg)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < colliderAttack.Count; i++)
        {
            Collider[] results = Physics.OverlapBox(colliderAttack[i].transform.position, colliderAttack[i].size, Quaternion.identity, mask);

            foreach (Collider objectColli in results)
            {
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage((float)Math.Round((damage + bonusdmg)/(i+1)), transform.root);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SkillAnim()
    {
        player.GetStuned(1.2f);
        yield return new WaitForSeconds(0.5f);
        PacticleSlash.SetActive(true);
        yield return new WaitForSeconds(1f);
        PacticleSlash.SetActive(false);

    }


    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }
}
