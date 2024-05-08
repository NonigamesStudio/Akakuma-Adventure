using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KingSlimeHealthBarUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject canvas;
    public float lifePercentage; 


    private void Start()
    {
        Hide();
        

    }

    private void Update()
    {
        lifePercentage = ((float)health.actualHealth / (float)health.maxHealth);
        barImage.fillAmount = lifePercentage;
        
        
    }


    public void Show()
    {
        canvas.SetActive(true);
    }

    public void Hide()
    {
        canvas.SetActive(false);
    }

}