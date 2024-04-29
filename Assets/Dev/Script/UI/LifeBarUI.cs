using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUI : MonoBehaviour
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
        
        if (lifePercentage == 1f|| lifePercentage<=0)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }


    private void Show()
    {
        canvas.enabled = true;
    }

    private void Hide()
    {
        canvas.enabled = false;
    }

}