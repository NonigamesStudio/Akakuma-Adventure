using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider barEnemiesStillAlive;
    [SerializeField] TextMeshProUGUI coinText_text;
    int score;

    private void OnEnable()
    {
        EnemyController.OnEnemyDeath += UpdateBarProgressWave;
        EnemyController.OnChangeWave += RestarBarProgressWave;

        Coin.OnCoinCollected += UpdateCoinsUI;
    }
    private void OnDisable()
    {
        EnemyController.OnEnemyDeath -= UpdateBarProgressWave;
        EnemyController.OnChangeWave -= RestarBarProgressWave;

        Coin.OnCoinCollected -= UpdateCoinsUI;
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

    void UpdateCoinsUI()
    {
        score++;
        coinText_text.text = "Coins: " + score;
    }
}
