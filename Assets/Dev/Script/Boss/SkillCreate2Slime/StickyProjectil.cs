using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyProjectil : MonoBehaviour
{
    [HideInInspector] public float dmg;
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider col;
    [SerializeField] float coolDown;
    [SerializeField] float duration;
    [SerializeField] GameObject VFXAcid;
    [SerializeField] MeshRenderer meshRenderer;
    

    public void Shoot()
    {
        col.enabled = false;
        rb.AddForce((transform.forward + Vector3.up) * Random.Range(4f,10f), ForceMode.Impulse);
        StartCoroutine(CoolDownTrigger());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            rb.isKinematic = true; // repos la el objeto para que calce justo con el piso
            col.isTrigger = true;
            meshRenderer.enabled = false;
            VFXAcid.SetActive(true);
            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDownTrigger()
    {
        yield return new WaitForSeconds(1);
        col.enabled = true;
    }
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
