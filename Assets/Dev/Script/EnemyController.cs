
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [Header("Refs Components")]
    [SerializeField] Transform playerTransform;
    [SerializeField] EnemyAI prefabEnemy;
    [Space(10)]
    [Header("Refs Pool Enemy")]
    [SerializeField] List<EnemyAI> enemysPool;
    [SerializeField] GameObject boss;
    [Space(10)]
    [Header("Variables Spawn Enemys")]
    [SerializeField] int amountEnemysConstantOnField;
    [SerializeField] float radioAreaToSpawn;
    [Space(10)]
    [Header("Variables Waves")]
    [SerializeField] List<int> numberOfEnemiesPerWave;
    [SerializeField] int secsBetweenWavesSpawn;

    [SerializeField] Animator rockAnim;

    [SerializeField] List<Transform> spanwPoints;


    public static System.Action<float,float> OnEnemyDeath;
    public static System.Action OnChangeWave;

    int currentWave = 0;
    int enemiesDeathInCurrentWave;

    private void Start()
    {
        foreach (EnemyAI enemy in enemysPool)
        {
            enemy.gameObject.SetActive(false);
            enemy.playert = playerTransform;
            enemy.enemyController = this;
        }
        StartCoroutine(SortSpawnPointsByDistance(playerTransform));
        SpawnWave();
    }



    public EnemyAI SpawnOneEnemy(Vector3 spawnPosition)

    {   
        transform.position=spawnPosition;
        Vector2 randomPos = Random.insideUnitCircle * radioAreaToSpawn;//genera un punto random en un radio de 10 unidades
        
       

        foreach (EnemyAI enemy in enemysPool)
        {
            if (enemy.gameObject.activeSelf) continue;
            enemy.agent.enabled = false;
            enemy.gameObject.SetActive(true);
            enemy.transform.position = new Vector3(randomPos.x,1.5f, randomPos.y)+spawnPosition; 
            enemy.transform.SetParent(null);
            enemy.agent.enabled = true;
            enemy.SetWalkingIdlePoints(spawnPosition);
            if (currentWave>2 && currentWave<=4)
            {
               SetEnemyForWave(enemy.gameObject,2);
            }else if (currentWave>4)
            {
                SetEnemyForWave(enemy.gameObject,3);
            }

            return enemy;
        }

        EnemyAI clonEnemy = Instantiate(prefabEnemy, transform);
        enemysPool.Add(clonEnemy);
        clonEnemy.agent.enabled = false;
        clonEnemy.gameObject.SetActive(false);
        clonEnemy.transform.position = new Vector3(randomPos.x,1.5f, randomPos.y)+spawnPosition;
        clonEnemy.gameObject.SetActive(true);
        clonEnemy.agent.enabled = true;
        clonEnemy.SetWalkingIdlePoints(spawnPosition);
        if (currentWave>2 && currentWave<=4)
            {
               SetEnemyForWave(clonEnemy.gameObject,2);
            }else if (currentWave>4)
            {
                SetEnemyForWave(clonEnemy.gameObject,3);
            }
        return clonEnemy;
    }

    public void EnemyDeath(EnemyAI enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(transform);

        OnEnemyDeath?.Invoke(numberOfEnemiesPerWave[currentWave-1], (numberOfEnemiesPerWave[currentWave - 1]-ReturnHowManyEnemiesStillAlive()));

        CheckEnemiesAliveAndStartNewWave();
    }

    void SpawnWave()
    {
        //if (currentWave >= numberOfEnemiesPerWave.Count) { rockAnim.Play("New Animation"); return; }
        LeanTween.delayedCall(secsBetweenWavesSpawn, () =>
        {
            spanwPoints.Sort((a, b) => //ordena la lista de puntos de spawn por distancia al jugador
            {
            float distA = Vector3.Distance(a.position, playerTransform.position);
            float distB = Vector3.Distance(b.position, playerTransform.position);
            return distA.CompareTo(distB);
            });
            OnChangeWave?.Invoke();
            currentWave++;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < numberOfEnemiesPerWave[currentWave-1]; j++)
                {
                SpawnOneEnemy(spanwPoints[i].position);
                }
            }
            
        });
    }

    void CheckEnemiesAliveAndStartNewWave()
    {
        foreach (EnemyAI enemy in enemysPool)
        {
            if (enemy.gameObject.activeSelf) return;
        }
        SpawnWave();
    }

    public int ReturnHowManyEnemiesStillAlive()
    {
        int count = 0;
        foreach (EnemyAI enemy in enemysPool)
        {
            if (enemy.gameObject.activeSelf) count++;
        }

        return count;
    }
    IEnumerator SortSpawnPointsByDistance(Transform target)
    {
        while (true)
        {
            
            spanwPoints.Sort((a, b) => 
            {
                float distA = Vector3.Distance(a.position, target.position);
                float distB = Vector3.Distance(b.position, target.position);
                return distA.CompareTo(distB);
            });
            yield return new WaitForSeconds(1f);
        }
    }

    void SetEnemyForWave(GameObject enemy, int waveNumber)
    {
        if(enemy.TryGetComponent<Health>(out Health health))
        {
            if (waveNumber == 2)
            {
                health.SetMaxHealth(25);

            }else if (waveNumber == 3)
            {
                health.SetMaxHealth(30);
            }
            
        }
        if(enemy.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
        {
            if (waveNumber == 2)
            {
                enemyAI.attackDmg = 2.5f;
                
            }else if (waveNumber == 3)
            {
                enemyAI.attackDmg = 3f;
            }
           
            
        }
        for (int i = 0; i <= 2; i++)
        {
            GameObject childObject = enemy.transform.GetChild(i).gameObject;
            childObject.SetActive(false);
        }
        GameObject childObjectToActivate = enemy.transform.GetChild(waveNumber-1).gameObject;
       
        childObjectToActivate.SetActive(true);

    }

   


}
