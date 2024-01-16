using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectil : MonoBehaviour
{
    [HideInInspector]public float dmg;
    [SerializeField] Rigidbody rb;

    public void Shoot()
    {
        rb.AddForce(transform.forward * 10, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           if( other.TryGetComponent<Health>(out Health playerHealth))
            {
                playerHealth.TakeDamage(dmg);
                Destroy(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
