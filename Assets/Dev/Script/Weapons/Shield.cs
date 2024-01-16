using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour,IWeapon
{
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] SphereCollider colliderSkill;
    [SerializeField] float skillDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDownSkill;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject skillParticle;
    bool isOnCoolDownSkill;
    private void Awake()
    {
        colliderSkill = GetComponent<SphereCollider>();
    }

    public void Attack(float bonusDmg)
    {
        
    }

    void SkillCoolDown()
    {
        LeanTween.delayedCall(coolDownSkill, () => { isOnCoolDownSkill = false; });
    }
    public void Skill()
    {
        if (isOnCoolDownSkill) return;
        isOnCoolDownSkill = true;
        SkillCoolDown();


        Collider[] results = Physics.OverlapSphere(transform.position, colliderSkill.radius, mask);

        foreach (Collider objectColli in results)
        {
            if (objectColli.TryGetComponent<Health>(out Health health))
            {
                if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(damage);
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
