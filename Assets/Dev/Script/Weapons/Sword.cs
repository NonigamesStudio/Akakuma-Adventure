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

    [SerializeField] bool isEnemy;
    [SerializeField] Player player;


    public void Attack(float bonusDmg)
    {
        if (!isOnCoolDown)
        {
            StartCoroutine(AttackAction(bonusDmg));
            if (!isEnemy) player.OnWeaponAttack?.Invoke();
            isOnCoolDown = true;
            
            LeanTween.delayedCall(0.4f, () => { spriteSlash.SetActive(true); });
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
        AudioManager.instance.PlayOneShot(FMODEvents.instance.wooshSound,this.transform.position);
        float time = 0;
        while (attackDuration > time)
        {
            if (isEnemy) yield return new WaitForSeconds(0.2f);
            else { yield return new WaitForSeconds(0.3f); }
            Collider[] results = Physics.OverlapBox(colliderAttack.transform.position, colliderAttack.size,Quaternion.identity, mask);

            if (results.Length <= 0) yield return null;

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
            Collider[] results = Physics.OverlapBox(colliderAttack.transform.position, colliderSkill.size, Quaternion.identity, mask);

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
        player.OnWeaponSkill?.Invoke();
        yield return new WaitForSeconds(0.3f);
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


