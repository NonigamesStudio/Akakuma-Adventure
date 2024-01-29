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
    public Animator anim;
    Rigidbody rb;
    Transform t;
    Health health;

    [Space(5)]
    [Header("Variables Enemy")]
    [SerializeField] float attackRange;
    [SerializeField] GameObject weapon;
    [SerializeField] float attackDmg;
    [SerializeField] float knockBackForce;

    [Space(5)]
    [Header("Variables Effects")]
    [SerializeField] GameObject deathParticle;
    [SerializeField] GameObject coinParticle;

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
        RestartParticles();
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
        StartParticleDeath();
        enemyController.EnemyDeath(this);
    }

    void AttackState()
    {
        agent.isStopped = true;
        
        anim.SetTrigger("Attack");
    }
    public void AttackWeapon()
    {
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
        anim.SetTrigger("GetDmg");
        agent.isStopped = true;
        agent.enabled = false;
        rb.AddForce(-t.forward * knockBackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        Vector3 temp = rb.velocity;
        LeanTween.value(gameObject, temp, Vector3.zero, 0.1f).setOnUpdate((Vector3 value) => { rb.velocity = value; });
        yield return new WaitForSeconds(0.1f);
        onKnockBack = false;
        agent.enabled = true;
        StartCoroutine(RestarEnemy());
    }

    IEnumerator RestarEnemy()
    {
        yield return new WaitForSeconds(5);
        rb.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.enabled = false;
        onKnockBack = false;
        agent.enabled = true;
    }

    void StartParticleDeath()
    {
        deathParticle.transform.SetParent(null);
        deathParticle.SetActive(true);
        Instantiate(coinParticle, transform.position, transform.rotation);
        LeanTween.delayedCall(2, () => { RestartParticles(); });
    }

    void RestartParticles()
    {
        deathParticle.transform.SetParent(transform);
        deathParticle.SetActive(false);
    }
}
