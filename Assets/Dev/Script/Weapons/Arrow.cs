using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    public Rigidbody rb;
    public Bow bow;
    public float dmg;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void ThrowArrow(float force)
    {
        rb.AddForce(bow.player.transform.forward * force,ForceMode.Impulse);
        transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent<Health>(out Health heatlh))
        {
            if (collision.gameObject.layer == gameObject.layer) return;
            heatlh.TakeDamage(dmg);
            ResetArrow();
        }
    }

    public void ResetArrow()
    {
        rb.velocity = Vector3.zero;
        transform.SetParent(bow.transform);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);
    }
    private void OnBecameInvisible()
    {
        ResetArrow();
    }
}
