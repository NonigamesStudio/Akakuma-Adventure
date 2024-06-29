using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleTick : MonoBehaviour
{
    Health health;
    [SerializeField] float duration;
    int layer;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    Color color;
    private void Awake()
    {
        layer = gameObject.layer;
        health = GetComponent<Health>();
        color = meshRenderer.material.color;
    }

    private void OnEnable()
    {
        health.OnLifeChange += StartInvincible;
    }

    private void OnDisable()
    {
        health.OnLifeChange -= StartInvincible;
    }

    void StartInvincible(Transform player)
    {
        StartCoroutine(InvincibleAction());
    }

    IEnumerator InvincibleAction()
    {
        gameObject.layer = 6;// dash
        meshRenderer.material.color = new Color(50, color.g, color.b);
        yield return new WaitForSeconds(duration);
        gameObject.layer = layer;
        meshRenderer.material.color = color;

    }
}
