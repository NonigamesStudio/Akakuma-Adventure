using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] float damage;
    [SerializeField] float maxDamage;
    [SerializeField] BoxCollider colliderAttack;
    [SerializeField] float attackDuration;
    [SerializeField] float tickTimeDmg;
    [SerializeField] float coolDown;
    [SerializeField] GameObject spriteSlash;
    [SerializeField] LayerMask mask;
    bool isOnCoolDown;
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position, colliderAttack.size);
    //}
}


