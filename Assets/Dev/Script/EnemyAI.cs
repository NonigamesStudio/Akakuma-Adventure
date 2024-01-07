using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Health))]
public class EnemyAI : MonoBehaviour
{
    [Header("Refs Component")]
    public NavMeshAgent agent;
    public Transform playert;
    public EnemyController enemyController;
    Rigidbody rb;
    Transform t;
    Health health;

    [Space(5)]
    [Header("Variables Enemy")]
    [SerializeField] float attackRange;
    [SerializeField] GameObject weapon;
    [SerializeField] float attackDmg;
    [SerializeField] float knockBackForce;

    IWeapon currentWeapon;
    bool onKnockBack;

    private void Awake()
    {
        t = GetComponent<Transform>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        currentWeapon = weapon.GetComponent<IWeapon>();
    }
    private void OnEnable()
    {
        agent.enabled = true;
        health.OnDeath += Dead;
        health.OnTakeDmg += Knockback;
    }
    private void OnDisable()
    {
        agent.enabled = false;
        health.OnDeath -= Dead;
        health.OnTakeDmg -= Knockback;
    }

    void Update()
    {
        if (onKnockBack) return;
        if (Vector3.Distance(t.position, playert.position) < attackRange) AttackState(); //Attack
        else FollowState();
    }

    void Dead()
    {
        enemyController.EnemyDeath(this);
    }

    void AttackState()
    {
        agent.isStopped = true;
        currentWeapon.Attack(attackDmg);
    }
    void FollowState()
    {
        agent.isStopped = false; 
        agent.destination = playert.position;
    }
    void Knockback()
    {
        onKnockBack = true;
        StartCoroutine(KnockbackCoroutine());
    }
    IEnumerator KnockbackCoroutine()
    {
        agent.isStopped = true;
        agent.enabled = false;
        rb.AddForce(-t.forward * knockBackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        Vector3 temp = rb.velocity;
        LeanTween.value(gameObject, temp, Vector3.zero, 0.1f).setOnUpdate((Vector3 value) => { rb.velocity = value; });
        yield return new WaitForSeconds(0.1f);
        onKnockBack = false;
        agent.enabled = true;
    }
}
