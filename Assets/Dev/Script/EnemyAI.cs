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
    Transform t;
    Health health;

    [Space(5)]
    [Header("Variables Enemy")]
    [SerializeField] float attackRange;
    [SerializeField] GameObject weapon;
    [SerializeField] float attackDmg;

    IWeapon currentWeapon;

    private void Awake()
    {
        t = GetComponent<Transform>();
        health = GetComponent<Health>();
        currentWeapon = weapon.GetComponent<IWeapon>();
    }
    private void OnEnable()
    {
        agent.enabled = true;
        health.OnDeath += Dead;
    }
    private void OnDisable()
    {
        agent.enabled = false;
        health.OnDeath -= Dead;
    }

    void Update()
    {
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
        Vector2 dir = (playert.position - t.position).normalized;
        t.up = dir;
    }
}
