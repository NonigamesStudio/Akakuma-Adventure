using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] Collider2D colliderAttack;
    [SerializeField] float attackDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDown;
    [SerializeField] GameObject spriteSlash;
    [SerializeField] LayerMask mask;
    bool isOnCoolDown;


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
        else Debug.Log("Is On CoolDown");
    }

    public void Skill()
    {
       
    }

    public void TurnOnOffWeapon(bool turnOnOff)
    {
        gameObject.SetActive(turnOnOff);
    }

    IEnumerator AttackAction(float bonusdmg)
    {
        float time = 0;
        while(attackDuration > time)
        {
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            List<Collider2D> results = new List<Collider2D>();
            Physics2D.OverlapCollider(colliderAttack, filter, results);

            foreach (Collider2D objectColli in results)
            {
                if (objectColli.TryGetComponent<Health>(out Health health))
                {
                    if(objectColli.gameObject.layer != gameObject.layer) health.TakeDamage(damage + bonusdmg);
                }

            }

            time += tickTimeDmg;

            yield return new WaitForSeconds(tickTimeDmg);
        }
    }
}
