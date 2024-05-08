using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour, IWeapon
{
    public Player player;

    [Header("Normal Attack Refs")]
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] BoxCollider colliderAttack;
    [SerializeField] float attackDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDown;
    [SerializeField] GameObject PacticleSlash;
    [SerializeField] LayerMask mask;

    [Space(5)]
    [Header("Skill Attack Refs")]
    [SerializeField] SphereCollider colliderSkill;
    [SerializeField] Transform pivotToSkill;
    [SerializeField] float coolDownSkill;
    [SerializeField] float skillDuration;
    [SerializeField] float speedRotSkill;
    [SerializeField] GameObject skillParticle;
    [SerializeField] bool isEnemy;



    bool isOnCoolDownNormalAttack;
    bool isOnCoolDownSkill;
    public void Attack(float bonusDmg)
    {
        if (!isOnCoolDownNormalAttack)
        {
            if (!isEnemy) AnimController_Player.ins.PlayAnim(AnimNamesPlayer.AttackScythe);
            StartCoroutine(AttackAction(bonusDmg));
            StartCoroutine(SkillAnim());
            if (!isEnemy) player.OnWeaponAttack?.Invoke();
            isOnCoolDownNormalAttack = true;
            //spriteSlash.SetActive(true);
            LeanTween.delayedCall(coolDown, () => { isOnCoolDownNormalAttack = false; });
            //LeanTween.delayedCall(attackDuration, () => { spriteSlash.SetActive(false); });
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
                    if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage + bonusdmg, transform.root);
                }
            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
        PacticleSlash.SetActive(false); ;
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
        player.onSkill = true;
        float time = 0;
        player.OnWeaponSkill?.Invoke();
        while (skillDuration > time)
        {
            Collider[] results = Physics.OverlapSphere(transform.position, colliderSkill.radius, mask);

            foreach (Collider objectColli in results)
            {
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage, transform.root);
                }
            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
        player.onSkill = false;
        pivotToSkill.localEulerAngles = Vector3.zero;
        SkillCoolDown();
    }

    IEnumerator SkillAnim()
    {
        player.GetStuned(1f);
        yield return new WaitForSeconds(0.5f);
        PacticleSlash.SetActive(true);
        yield return new WaitForSeconds(1f);
        PacticleSlash.SetActive(false);

        //skillParticle.SetActive(true);

        //float time = skillDuration;
        //float ytemp = 0;
        //while (time>0)
        //{
        //    pivotToSkill.localEulerAngles =new Vector3(0, ytemp*10, 0);
        //    ytemp += Time.deltaTime * speedRotSkill;
        //    time -= Time.deltaTime;
        //    yield return null;
        //}
        //pivotToSkill.localEulerAngles = Vector3.zero;

        //skillParticle.SetActive(false);
    }

    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }
}
