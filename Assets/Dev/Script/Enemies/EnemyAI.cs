using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
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
    public Health health;
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    [Space(5)]
    [Header("Variables Enemy")]
    [SerializeField] float attackRange;
    [SerializeField] float maxDistDetection = 15;
    [SerializeField] float waitTimeUntilLostPlayer = 7;
    [SerializeField] GameObject weapon;
    public float attackDmg;
    [SerializeField] float knockBackForce;
    [SerializeField] Material[] matsList;

    [Space(5)]
    [Header("Variables Effects")]
    [SerializeField] GameObject deathParticle;
    [SerializeField] GameObject coinParticle;
    [SerializeField] GameObject getHitParticle;



    IWeapon currentWeapon;
    bool onKnockBack;
    List <Vector2> walkingIdlePoints = new List<Vector2>();
    float waitTime;
    int patrolPoints;
    float timeToChangePatrolPoint;
    Vector3 playerLastDetectedPosition;
    float timeSinceLastDetection;
    int layerMask;



    

    private enum State
    {
        Idle,
        Attack
    }
    private State currentState;

    private void Awake()
    {
        t = GetComponent<Transform>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        currentWeapon = weapon.GetComponent<IWeapon>();
    }
    private void OnEnable()
    {
        currentState = State.Idle;
        agent.enabled = true;
        health.OnDeath += Dead;
        health.OnTakeDmg += Knockback;
        RestartParticles();
        walkingIdlePoints.Clear();
        InvokeRepeating ("RestarEnemy", 0, 0.5f);
        layerMask = 1 << LayerMask.NameToLayer("Enemy");
        layerMask = ~layerMask; 
    }
    private void OnDisable()
    {
        agent.enabled = false;
        health.OnDeath -= Dead;
        health.OnTakeDmg -= Knockback;
    }


    public void ActiveMesh(int t)
    {
        if (t >= matsList.Length) return;

        meshRenderer.material = matsList[t];
    }

    void Update()
    {
       
        if (onKnockBack) return;
        anim.SetFloat("Walk", agent.velocity.magnitude);

        
        if (Vector3.Distance(t.position, playert.position) < maxDistDetection)
        { 
            timeSinceLastDetection=0;
            currentState = State.Attack;
        }
       
        
        switch (currentState)
        {     
            case State.Idle:
                {
               
                if (Vector3.Distance(transform.position, new Vector3(walkingIdlePoints[patrolPoints].x, t.position.y, walkingIdlePoints[patrolPoints].y)) > 1.5f)
                {
                    FollowState(new Vector3(walkingIdlePoints[patrolPoints].x, t.position.y, walkingIdlePoints[patrolPoints].y));
                    timeToChangePatrolPoint = 0;
                    waitTime = 0; 
                }
                else if (timeToChangePatrolPoint <= waitTime) 
                {
                    if (waitTime == 0)
                    {
                        waitTime = Random.Range(5,10); 
                    }
                    timeToChangePatrolPoint += Time.deltaTime;
                }
                else 
                {
                    patrolPoints++;
                }
                if (patrolPoints >= walkingIdlePoints.Count) patrolPoints = 0; 
                break;
                }
            case State.Attack:

                if(Vector3.Distance(t.position, playert.position) < attackRange)
                {
                    AttackState();
                    break;
                }

                if (Physics.Raycast(transform.position,playert.position-transform.position,out RaycastHit hit, maxDistDetection, layerMask))
                {   
                    if (hit.transform.CompareTag("Player")&& Vector3.Distance(transform.position, playert.position) <= maxDistDetection) 
                    {
                        playerLastDetectedPosition=playert.position;
                        timeSinceLastDetection=0;   
                    }
                }

                if(timeSinceLastDetection<waitTimeUntilLostPlayer)
                {
                    timeSinceLastDetection += Time.deltaTime;
                    FollowState(playerLastDetectedPosition);
                }else 
                {
                    currentState = State.Idle;   
                }
                break;
        }
    }

    void Dead()
    {
        StartParticleDeath();
        enemyController.EnemyDeath(this);
    }

    void AttackState()
    {
        transform.LookAt(playert.position);
        agent.isStopped = true;
        anim.SetTrigger("Attack");
    }
    public void AttackWeapon()
    {
        currentWeapon.Attack(attackDmg);
    }
    void FollowState(Vector3 target)
    {
        agent.isStopped = false; 
        agent.destination = target;
    }
    void Knockback(Transform player)
    {
        getHitParticle.SetActive(true);
        onKnockBack = true;
        StartCoroutine(KnockbackCoroutine(player));
    }
    IEnumerator KnockbackCoroutine(Transform player)
    {
        anim.SetTrigger("GetDmg");
        agent.isStopped = true;
        agent.enabled = false;
        rb.AddForce(player.forward * knockBackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        Vector3 temp = rb.velocity;
        LeanTween.value(gameObject, temp, Vector3.zero, 0.1f).setOnUpdate((Vector3 value) => { rb.velocity = value; });
        yield return new WaitForSeconds(0.1f);
        onKnockBack = false;
        agent.enabled = true;
        getHitParticle.SetActive(false);
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

    public void SetWalkingIdlePoints(Vector3 spawnPoint)
    {
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            Vector2 randomPos = (Random.insideUnitCircle * 5) + new Vector2(spawnPoint.x, spawnPoint.z);
            walkingIdlePoints.Add(randomPos);
        }
        patrolPoints = 0;
    }

    public void SwitchToAttackState()
    {
        playerLastDetectedPosition=playert.position;
        timeSinceLastDetection=0;
        currentState = State.Attack;
        
    }

    public void SearchAndSetNearbyAllys()
    {
       
        float radius = 7;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<EnemyAI>(out EnemyAI enemyAI) && hitCollider.gameObject != gameObject)
            {
                
                enemyAI.SwitchToAttackState();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
