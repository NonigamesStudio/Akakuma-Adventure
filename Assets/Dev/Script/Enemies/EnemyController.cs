
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [Header("Refs Components")]
    [SerializeField] Transform playerTransform;
    [SerializeField] EnemyAI prefabEnemy;
    [SerializeField] EnemyAI prefabEnemyDist;
    [Space(10)]
    [Header("Refs Pool Enemy")]
    [SerializeField] List<EnemyAI> enemysPool;
    [SerializeField] List<EnemyAI> enemysDistancesPool;
    [SerializeField] GameObject boss;
    [Space(10)]
    [Header("Variables Spawn Enemys")]
    [SerializeField] int amountEnemysConstantOnField;
    [SerializeField] float radioAreaToSpawn;
    [Space(10)]
    [Header("Variables Waves")]
    //[SerializeField] List<int> numberOfEnemiesPerWave;
    [SerializeField] List<DataWaveEnemy> waveDataList;
    [SerializeField] int secsBetweenWavesSpawn;

    [SerializeField] Animator rockAnim;
    //[SerializeField] List<Transform> spanwPoints;

    [SerializeField] GameObject doorA;
    [SerializeField] GameObject doorB;

    Vector3 spanwPointClosest;

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
        
        foreach (EnemyAI enemy in enemysDistancesPool)
        {
            enemy.gameObject.SetActive(false);
            enemy.playert = playerTransform;
            enemy.enemyController = this;
        }


        //StartCoroutine(SortSpawnPointsByDistance(playerTransform));
        SpawnWave();
    }



    public EnemyAI SpawnOneEnemy(DataTypeSpawnEnemy data)
    {   
        Vector2 randomPos = Random.insideUnitCircle * radioAreaToSpawn;//genera un punto random en un radio de 10 unidades
        randomPos.x += data.position.position.x;
        randomPos.y += data.position.position.z;

        if(data.isDistance)
        {
            foreach (EnemyAI enemy in enemysDistancesPool)
            {
                if (enemy.gameObject.activeSelf) continue;

                enemy.gameObject.SetActive(true);
                enemy.agent.enabled = false;
                enemy.transform.SetParent(null);
                enemy.transform.position = new Vector3(randomPos.x, 5f, randomPos.y);
                enemy.agent.enabled = true;
                enemy.SetWalkingIdlePoints(data.position.position);
                SetEnemyForWave(enemy, data.healthMax, data.attackdmg, data.typeEnemy);

                return enemy;
            }
        }

        foreach (EnemyAI enemy in enemysPool)
        {
            if (enemy.gameObject.activeSelf) continue;

            enemy.gameObject.SetActive(true);
            enemy.agent.enabled = false;
            enemy.transform.SetParent(null);
            enemy.transform.position = new Vector3(randomPos.x, 5f, randomPos.y);
            enemy.agent.enabled = true;
            enemy.SetWalkingIdlePoints(data.position.position);
            SetEnemyForWave(enemy, data.healthMax, data.attackdmg, data.typeEnemy);

            return enemy;
        }



        EnemyAI clonEnemy = Instantiate(prefabEnemy, transform);
        enemysPool.Add(clonEnemy);
        clonEnemy.agent.enabled = false;
        clonEnemy.gameObject.SetActive(false);
        clonEnemy.transform.position = new Vector3(randomPos.x,1.5f, randomPos.y);
        clonEnemy.gameObject.SetActive(true);
        clonEnemy.agent.enabled = true;
        clonEnemy.SetWalkingIdlePoints(data.position.position);
       

        return clonEnemy;
    }

    public void EnemyDeath(EnemyAI enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(transform);

        //OnEnemyDeath?.Invoke(waveDataList[currentWave-1], (waveDataList[currentWave - 1]-ReturnHowManyEnemiesStillAlive()));

        CheckEnemiesAliveAndStartNewWave();
    }

    void SpawnWave()
    {
        if (currentWave >= waveDataList.Count) 
        { 
            AnimDoor(); 
        }
        else
        {
            LeanTween.delayedCall(secsBetweenWavesSpawn, () =>
            {
                OnChangeWave?.Invoke();
                foreach (DataTypeSpawnEnemy enemyData in waveDataList[currentWave].enemyToSpawn)
                {
                    SpawnOneEnemy(enemyData);
                }
                
                currentWave++;
            });
        }
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



    //IEnumerator SortSpawnPointsByDistance(Transform target)
    //{
    //    while (true)
    //    {
            
    //        spanwPoints.Sort((a, b) => 
    //        {
    //            float distA = Vector3.Distance(a.position, target.position);
    //            float distB = Vector3.Distance(b.position, target.position);
    //            return distA.CompareTo(distB);
    //        });
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    void SetEnemyForWave(EnemyAI enemy, float health, float attack, TypeEnemy typeEnemy)
    {
        enemy.health.SetMaxHealth(health);
        enemy.attackDmg = attack;
        enemy.ActiveMesh((int)typeEnemy);
    }
    public void AnimDoor()
    {
        Debug.Log("AnimDoor");
        LeanTween.rotateY(doorA, -220, 0.5f);
        LeanTween.rotateY(doorB, 220, 0.5f);
    }

}

[System.Serializable]
public class DataWaveEnemy
{
    public DataTypeSpawnEnemy[] enemyToSpawn;
}
[System.Serializable]
public class DataTypeSpawnEnemy
{
    public bool isDistance;
    public Transform position;
    public float healthMax;
    public float attackdmg;
    public TypeEnemy typeEnemy;
}

public enum TypeEnemy //Mantener Orden
{
    Verde,
    Azul,
    Rojo
}