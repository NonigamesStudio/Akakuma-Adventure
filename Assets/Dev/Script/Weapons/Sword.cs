using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] BoxCollider colliderAttack;
    [SerializeField] BoxCollider colliderSkill;
    [SerializeField] float attackDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDown;
    [SerializeField] float coolDownSkill;
    [SerializeField] float skillDuration;
    [SerializeField] GameObject spriteSlash;
    [SerializeField] GameObject spriteSlashSkill;
    [SerializeField] LayerMask mask;
    bool isOnCoolDown;
    bool isOnCoolDownSkill;
    private void Awake()
    {
        colliderAttack = GetComponent<BoxCollider>();
    }

    public void Attack(float bonusDmg)
    {
        if (!isOnCoolDown)
        {
            StartCoroutine(AttackAction(bonusDmg));
            isOnCoolDown = true;
            spriteSlash.SetActive(true);
            LeanTween.delayedCall(coolDown, () => { isOnCoolDown = false; });
            LeanTween.delayedCall(attackDuration, () => { spriteSlash.SetActive(false); });
        }
        else { //on cooldown
               }
    }
    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }


    IEnumerator AttackAction(float bonusdmg)
    {
        float time = 0;
        while (attackDuration > time)
        {
            Collider[] results = Physics.OverlapBox(transform.position, colliderAttack.size,Quaternion.identity, mask);


            foreach (Collider objectColli in results)
            {
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if(gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage + bonusdmg);
                }
            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
    }

    public void Skill()
    {
        if (isOnCoolDownSkill) return;
        isOnCoolDownSkill = true;
        StartCoroutine(SkillAnim());
        StartCoroutine(SkillAction());
    }

    void SkillCoolDown()
    {
        LeanTween.delayedCall(coolDownSkill, () => { isOnCoolDownSkill = false; });
    }

    IEnumerator SkillAction()
    {
        float time = 0;
        while (skillDuration > time)
        {
            Collider[] results = Physics.OverlapBox(transform.position, colliderSkill.size, Quaternion.identity, mask);

            foreach (Collider objectColli in results)
            {
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage);
                }
            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
        SkillCoolDown();
    }

    IEnumerator SkillAnim()
    {
        float time = skillDuration;
        while (time > 0)
        {
            spriteSlashSkill.SetActive(true);
            time -= Time.deltaTime;
            yield return null;
        }
        spriteSlashSkill.SetActive(false);
    }

}


