using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackBoss : MonoBehaviour
{
    [SerializeField] float fireRate;
    [SerializeField] LayerMask mask;
    [SerializeField] float dmg;
    [SerializeField] bool isOnDistance;
    [SerializeField] float time;

    private void Update()
    {
        if(isOnDistance)
        {
            time += Time.deltaTime;

            if(time > fireRate)
            {
                Collider[] results = Physics.OverlapSphere(transform.position, 6);

                foreach (Collider objectColli in results)
                {
                    if (objectColli.TryGetComponent<Health>(out Health health))
                    {
                        if (gameObject.layer != objectColli.gameObject.layer) health.TakeDamage(dmg);
                    }
                }

                time = 0;
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isOnDistance = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isOnDistance = false;
    }

}
