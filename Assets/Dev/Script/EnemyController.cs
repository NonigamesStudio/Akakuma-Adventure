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

        SpawnWave();
    }



    public EnemyAI SpawnOneEnemy()
    {
        Vector2 randomPos = Random.insideUnitCircle * radioAreaToSpawn;

        foreach (EnemyAI enemy in enemysPool)
        {
            if (enemy.gameObject.activeSelf) continue;

            enemy.gameObject.SetActive(true);
            enemy.transform.position = new Vector3(randomPos.x,1.5f, randomPos.y) ;
            enemy.transform.SetParent(null);
            return enemy;
        }

        EnemyAI clonEnemy = Instantiate(prefabEnemy, transform);
        enemysPool.Add(clonEnemy);
        clonEnemy.transform.position = randomPos;
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
        if (currentWave >= numberOfEnemiesPerWave.Count) { rockAnim.Play("New Animation"); return; }
        LeanTween.delayedCall(secsBetweenWavesSpawn, () =>
        {
            OnChangeWave?.Invoke();
            for (int i = 0; i < numberOfEnemiesPerWave[currentWave]; i++)
            {
                SpawnOneEnemy();
            }
            currentWave++;
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


}
