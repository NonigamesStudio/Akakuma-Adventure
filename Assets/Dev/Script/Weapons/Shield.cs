using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour,IWeapon
{
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] SphereCollider colliderSkill;
    [SerializeField] BoxCollider colliderAttack;
    [SerializeField] float skillDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDownSkill;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject skillParticle;
    [SerializeField] GameObject spriteSlash;
    [SerializeField] float coolDown;
    [SerializeField] float attackDuration;
    [SerializeField] Player player;
    [SerializeField] bool isEnemy;


    bool isOnCoolDownSkill;
    bool isOnCoolDownNormalAttack;
    private void Awake()
    {
        colliderSkill = GetComponent<SphereCollider>();
    }

    public void Attack(float bonusDmg)
    {
        if (!isOnCoolDownNormalAttack)
        {
            if (!isEnemy) AnimController_Player.ins.PlayAnim(AnimNamesPlayer.AttackShield);
            player.GetStuned(1.5f);
            StartCoroutine(AttackAction(bonusDmg));
            player.OnWeaponAttack?.Invoke();
            isOnCoolDownNormalAttack = true;
            LeanTween.delayedCall(1, () => {;skillParticle.SetActive(true);});
            LeanTween.delayedCall(2, () => {;skillParticle.SetActive(false);});
            LeanTween.delayedCall(2, () => {;skillParticle.SetActive(false);});
            //spriteSlash.SetActive(true);
            LeanTween.delayedCall(1.5f, () => { player.TakeBackSword(); });
            
            //LeanTween.delayedCall(attackDuration, () => { spriteSlash.SetActive(false); });
        }
        else
        { //on cooldown
        }
    }

    IEnumerator AttackAction(float bonusdmg)
    {
        yield return new WaitForSeconds(1f);
        float time = 0;
        while (attackDuration > time)
        {

            Collider[] results = Physics.OverlapBox(transform.position, colliderAttack.size, Quaternion.identity, mask);


            foreach (Collider objectColli in results)
            {
                if (objectColli.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
                {
                   
                enemyAI.SlowDown(5f);
                
                }
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage + bonusdmg, transform.root);
                }
            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
    }

    void SkillCoolDown()
    {
        LeanTween.delayedCall(coolDownSkill, () => { isOnCoolDownSkill = false; });
    }
    public void Skill()
    {
        if (isOnCoolDownSkill) return;
        isOnCoolDownSkill = true;
        player.OnWeaponSkill?.Invoke();
        SkillCoolDown();


        Collider[] results = Physics.OverlapSphere(transform.position, colliderSkill.radius, mask);

        foreach (Collider objectColli in results)
        {
            if (objectColli.TryGetComponent<Health>(out Health health))
            {
                if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage, transform.root);
            }
        }


        skillParticle.SetActive(true);

        LeanTween.delayedCall(2, () => { skillParticle.SetActive(false); });
    }



    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }

}
