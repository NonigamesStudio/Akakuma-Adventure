using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider barEnemiesStillAlive;

    private void OnEnable()
    {
        EnemyController.OnEnemyDeath += UpdateBarProgressWave;
        EnemyController.OnChangeWave += RestarBarProgressWave;
    }
    private void OnDisable()
    {
        EnemyController.OnEnemyDeath -= UpdateBarProgressWave;
        EnemyController.OnChangeWave -= RestarBarProgressWave;
    }

    void UpdateBarProgressWave(float enemiesSpawned,float enemiesStillAlive)
    {
        float temp = barEnemiesStillAlive.value;
        LeanTween.value(temp, (enemiesStillAlive / enemiesSpawned), 1).setOnUpdate((float value) => { barEnemiesStillAlive.value = value;});
    }
    void RestarBarProgressWave()
    {
        barEnemiesStillAlive.value = 0;
    }
}
