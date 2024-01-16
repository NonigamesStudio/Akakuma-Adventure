using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleTick : MonoBehaviour
{
    Health health;
    [SerializeField] float duration;
    int layer;
    MeshRenderer meshRenderer;
    private void Awake()
    {
        layer = gameObject.layer;
        health = GetComponent<Health>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        health.OnTakeDmg += StartInvincible;
    }

    private void OnDisable()
    {
        health.OnTakeDmg -= StartInvincible;
    }

    void StartInvincible()
    {
        StartCoroutine(InvincibleAction());
    }

    IEnumerator InvincibleAction()
    {
        gameObject.layer = 6;// dash
        Color color = meshRenderer.material.color;
        meshRenderer.material.color = new Color(50, color.g, color.b);
        yield return new WaitForSeconds(duration);
        gameObject.layer = layer;
        meshRenderer.material.color = color;

    }
}
