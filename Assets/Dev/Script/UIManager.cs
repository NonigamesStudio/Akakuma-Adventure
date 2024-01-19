using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Slider barEnemiesStillAlive;
    [SerializeField] TextMeshProUGUI coinText_text;
    [SerializeField] Health playerH;
    [SerializeField] Health bossH;
    [SerializeField] GameObject panelLose;
    [SerializeField] GameObject panelWin;

    int score;

    private void OnEnable()
    {
        EnemyController.OnEnemyDeath += UpdateBarProgressWave;
        EnemyController.OnChangeWave += RestarBarProgressWave;
        bossH.OnDeath += WinPanel;
        playerH.OnDeath += LosePanel;

        Coin.OnCoinCollected += UpdateCoinsUI;
    }
    private void OnDisable()
    {
        EnemyController.OnEnemyDeath -= UpdateBarProgressWave;
        EnemyController.OnChangeWave -= RestarBarProgressWave;
        bossH.OnDeath -= WinPanel;
        playerH.OnDeath -= LosePanel;

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

    void WinPanel()
    {
        panelWin.SetActive(true);
        Time.timeScale = 0;
    }
    void LosePanel()
    {
        panelLose.SetActive(true);
        Time.timeScale = 0;
    }
}
