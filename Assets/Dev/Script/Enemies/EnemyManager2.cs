using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager2 : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject cube;
    [SerializeField] Transform spawnPoint;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity); 
            Instantiate(cube, spawnPoint.position, Quaternion.identity); 
            enemy.transform.position = spawnPoint.position;
        }
    }
}
