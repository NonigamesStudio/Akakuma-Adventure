using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public event System.Action OnRainStarts;
    public event System.Action OnRainEnds;
    [SerializeField] private GameObject rainFall;
    [Range(0, 100)]
    [SerializeField] float rainingChance = 50;
    [SerializeField] int lenghtOfRainAndChecks = 25;
    float time = 0;

    void Update()
    {
        time = time + Time.deltaTime;
        if (time > lenghtOfRainAndChecks)
        {

            time = 0;
            if (WheaterChange())
            {
               
                OnRainStarts?.Invoke();
                rainFall.SetActive(true);
            }
            else
            {   
               
                OnRainEnds?.Invoke();
                rainFall.SetActive(false);
            }
        }
    }

    private bool WheaterChange()
    {
        float random = Random.Range(0, 100);

        if (random > rainingChance)
        {
            return false;
        }else
        {
            return true;
        }
        
    }
}
