using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour
{
    public Rigidbody rb;
    public Bow bow;
    public float dmg;

    [Header("VFX")]
    public GameObject explosion_Collision;
    public GameObject arrow_VFX;

    private void OnEnable()
    {
        StopCoroutine(ResetArrowColdDown());
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void ThrowArrow(float force)
    {
        rb.AddForce(bow.transform.forward * force,ForceMode.Impulse);
        transform.SetParent(null);
        StartCoroutine(ResetArrowColdDown());
    }

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.TryGetComponent<Health>(out Health heatlh))
        {
            if (collision.gameObject.layer == gameObject.layer) return;
            arrow_VFX.SetActive(false);
            explosion_Collision.SetActive(true);
            rb.velocity = Vector3.zero;
            //LeanTween.delayedCall(1, () => { ResetArrow(); });

            heatlh.TakeDamage(dmg, transform.root);
            ResetArrow();
        }
    }

    public void ResetArrow()
    {

        explosion_Collision.SetActive(false);
        arrow_VFX.SetActive(true);
        rb.velocity = Vector3.zero;
        transform.SetParent(bow.transform);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        gameObject.SetActive(false);
    }


    IEnumerator ResetArrowColdDown()
    {
        yield return new WaitForSeconds(5);
        ResetArrow();
    }
}
