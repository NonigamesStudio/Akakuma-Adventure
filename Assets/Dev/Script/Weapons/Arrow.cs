using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    public Bow bow;
    public float dmg;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    public void ThrowArrow(float force)
    {
        transform.SetParent(null);
        rb.AddForce(bow.transform.up * force,ForceMode2D.Impulse);
        transform.up = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Health>(out Health heatlh))
        {
            heatlh.TakeDamage(dmg);
            bow.ReturnArrowToPool(this);
        }
    }
    private void OnBecameInvisible()
    {
        bow.ReturnArrowToPool(this);
    }
}
