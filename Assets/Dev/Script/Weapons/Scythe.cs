using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour, IWeapon
{
    [Header("Normal Attack Refs")]
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] BoxCollider colliderAttack;
    [SerializeField] float attackDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDown;
    [SerializeField] GameObject spriteSlash;
    [SerializeField] LayerMask mask;

    [Space(5)]
    [Header("Skill Attack Refs")]
    [SerializeField] SphereCollider colliderSkill;
    [SerializeField] Transform pivotToSkill;
    [SerializeField] float skillDuration;
    [SerializeField] float speedRotSkill;



    bool isOnCoolDownNormalAttack;
    bool isOnCoolDownSkill;
    public void Attack(float bonusDmg)
    {
        if (!isOnCoolDownNormalAttack)
        {
            StartCoroutine(AttackAction(bonusDmg));
            isOnCoolDownNormalAttack = true;
            spriteSlash.SetActive(true);
            LeanTween.delayedCall(coolDown, () => { isOnCoolDownNormalAttack = false; });
            LeanTween.delayedCall(attackDuration, () => { spriteSlash.SetActive(false); });
        }
        else
        { //on cooldown
        }
    }

    IEnumerator AttackAction(float bonusdmg)
    {
        float time = 0;
        while (attackDuration > time)
        {
            Collider[] results = Physics.OverlapBox(transform.position, colliderAttack.size, Quaternion.identity, mask);


            foreach (Collider objectColli in results)
            {
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage + bonusdmg);
                }

            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
    }

    public void Skill()
    {
        StartCoroutine(SkillAnim());
        StartCoroutine(SkillAction());
    }

    IEnumerator SkillAction()
    {
        float time = 0;
        while (skillDuration > time)
        {
            Collider[] results = Physics.OverlapSphere(transform.position, colliderSkill.radius, mask);


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
    }

    IEnumerator SkillAnim()
    {
        float time = skillDuration;
        float ytemp = 0;
        while (time>0)
        {
            pivotToSkill.localEulerAngles =new Vector3(0, ytemp*10, 0);
            ytemp += Time.deltaTime * speedRotSkill;
            time -= Time.deltaTime;
            yield return null;
        }
        pivotToSkill.localEulerAngles = Vector3.zero;
    }

    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }
}
