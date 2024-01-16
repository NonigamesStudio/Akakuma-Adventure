using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBossEnemy : MonoBehaviour
{
    [SerializeField] Transform playerT;
    [SerializeField] Transform bossT;
    [SerializeField] StickyProjectil stickyProjectil;
    Health health;

    [SerializeField] float fireRate;
    float time = 0;
    private void Awake()
    {
        health = GetComponent<Health>();

    }
    private void OnEnable()
    {
        health.OnDeath += OnDeathEnemy;
        transform.SetParent(null);
    }
    private void OnDisable()
    {
        health.OnDeath -= OnDeathEnemy;
        transform.SetParent(bossT);
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        Vector3 dir = playerT.position - transform.position;
        transform.forward = dir.normalized;

        time += Time.deltaTime;

        if(time>= fireRate)
        {
            time = 0;
            StickyProjectil clon = Instantiate(stickyProjectil, transform.position,transform.rotation);
            clon.Shoot();
        }
    }



    void OnDeathEnemy()
    {
        gameObject.SetActive(false);
    }

}
