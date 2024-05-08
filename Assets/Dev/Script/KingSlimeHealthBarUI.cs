using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KingSlimeHealthBarUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image barImage;
    [SerializeField] private Canvas canvas;
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
        canvas.enabled = true;
    }

    public void Hide()
    {
        canvas.enabled = false;
    }

}