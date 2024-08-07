using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour, SceneChanger
{
    [SerializeField] Slider barEnemiesStillAlive;
    [SerializeField] TextMeshProUGUI coinText_text;
    [SerializeField] Health playerH;
    [SerializeField] Health bossH;
    [SerializeField] GameObject panelLose;
    [SerializeField] GameObject panelWin;
    [SerializeField] List<GameObject> healthBar;
    private int listHealth;
    [SerializeField] GameObject dashBar;
    [SerializeField] Sprite emptyImg;
    [SerializeField] Sprite fullImg;
    [SerializeField] Inventory playerInventory;
    [SerializeField] GameObject weaponGO;
    [SerializeField] List<Sprite> weaponSprite;

    [SerializeField] GameObject pruebaPortal;

    int hpThreshold;
    //int score;

    public Animator transitionUI;

    private void OnEnable()
    {
        EnemyController.OnEnemyDeath += UpdateBarProgressWave;
        EnemyController.OnChangeWave += RestarBarProgressWave;
        if (bossH != null)
        {
            bossH.OnDeath += WinPanel;
        }
        playerH.OnDeath += LosePanel;
        playerH.OnLifeChange += UpdateHealthUI;
        //Coin.OnCoinCollected -= UpdateCoinsUI;
        playerInventory.OnItemListChange += UpdateCoinsUI;
        TransactionManager.OnTransactionEnds += UpdateCoinsUI;

        
    }

    private void Start()
    {
        transitionUI.Play("TransitionImgAnim");
    }
    private void OnDisable()
    {
        EnemyController.OnEnemyDeath -= UpdateBarProgressWave;
        EnemyController.OnChangeWave -= RestarBarProgressWave;
        if (bossH != null)
        {
            bossH.OnDeath -= WinPanel;
        }
        playerH.OnDeath -= LosePanel;
        //Coin.OnCoinCollected -= UpdateCoinsUI;
        playerInventory.OnItemListChange += UpdateCoinsUI;
        TransactionManager.OnTransactionEnds -= UpdateCoinsUI;
        playerH.OnLifeChange -= UpdateHealthUI;

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
        //score++;
        coinText_text.text = "Souls: " + playerInventory.soulsCount.ToString();
    }

    void WinPanel()
    {
        //panelWin.SetActive(true); descomentar luego prueba portal
        // Time.timeScale = 0;
        pruebaPortal.SetActive(true);
    }
    void LosePanel()
    {
        LeanTween.delayedCall(3, () => {
            panelLose.SetActive(true);
            Time.timeScale = 0;
        });
    }

    void UpdateHealthUI(Transform player)
    {

        // for (int i = 0; i < (5-Mathf.RoundToInt(playerH.actualHealth * 5 / 100)); i++)
        // {
        //     healthBar[i].GetComponent<Image>().sprite = emptyImg;
        // }
        foreach (GameObject health in healthBar)
        {
            health.GetComponent<Image>().sprite = emptyImg;
        }
        float lifePercentageLeft = playerH.actualHealth / playerH.maxHealth;
        int lifeBarIndex = Mathf.RoundToInt(lifePercentageLeft * healthBar.Count);
        if (lifeBarIndex > healthBar.Count) lifeBarIndex = healthBar.Count; 
        for (int i = healthBar.Count - 1; i >= healthBar.Count - lifeBarIndex; i--)
        {
            healthBar[i].GetComponent<Image>().sprite = fullImg;
        }


    }
   

    public void ChangeWeaponSpriteAbility(int weaponNumber)
    {
        weaponGO.GetComponent<Image>().sprite = weaponSprite[weaponNumber];
    }


    public void ContinueButton()// Send to the main island when the player lose
    {
       Time.timeScale=1;
       ChangeScenes(GameManager.scenes.DevIsla2, new List<GameManager.scenes> {
                GameManager.scenes.ArtIsla2,GameManager.scenes.DevIsla2});
    }

    public void ChangeScenes(GameManager.scenes sceneToSetActive, List<GameManager.scenes> scenesLoad, List<GameManager.scenes> scenesUnload = null)
    {
        GameManager.instance.ChangeScenes(sceneToSetActive, scenesLoad, scenesUnload);
    }
}
