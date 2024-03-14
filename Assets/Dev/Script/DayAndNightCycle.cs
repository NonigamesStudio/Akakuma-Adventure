using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] bool isDay;
    [Header("Time Settings")]
    [Range(0f, 24f)]
    [SerializeField] float currentTime;
    [SerializeField] float timeSpeed = 1f;

    [Header("CurrentTime")]
    [SerializeField] string currentTimeString;

    [Header("Light Settings")]
    [SerializeField] Light sunLight;
    [SerializeField] float sunPosition = 1;
    [SerializeField] float sunIntensity = 1;
    [SerializeField] AnimationCurve sunIntensityMultiplier;
    [Header("Light Color")]
    [SerializeField] AnimationCurve sunColorCurve;
    [SerializeField] Gradient sunLightColor;
    [SerializeField] AnimationCurve ambientColorCurve;
    [SerializeField] Gradient ambientColor;

    // Start is called before the first frame update
    void Start()
    {
        CheckShadowStatus();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += timeSpeed * Time.deltaTime;

        if (currentTime >= 24f)
        {
            currentTime = 0f;
        }

        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
        
    }

    private void OnValidate()
    {
        UpdateLight();
        CheckShadowStatus();
    }

    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ":" + ((currentTime % 1) * 60).ToString("00");
    }

    void UpdateLight()
    {
        float sunRotation = (currentTime / 24f) * 360;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation-90f,sunPosition,0f);
        float nomalizedTime = currentTime / 24f;
        float intensityCurve= sunIntensityMultiplier.Evaluate(nomalizedTime);
        sunLight.intensity = sunIntensity * intensityCurve;

        float sunColorMultiplier = sunColorCurve.Evaluate(nomalizedTime);
        sunLight.color = sunLightColor.Evaluate(sunColorMultiplier);
        
        float ambientColorMultiplier = ambientColorCurve.Evaluate(nomalizedTime);
        RenderSettings.ambientLight =  ambientColor.Evaluate(ambientColorMultiplier);

        
    }

    void CheckShadowStatus()
    {
        float currentSunRotation = currentTime;
        if (currentSunRotation >= 5 && currentSunRotation <= 19)
        {
            sunLight.shadows = LightShadows.Soft;
            isDay = true; 
        
        }
        else
        {
            sunLight.shadows = LightShadows.None;
            isDay = false; 
        }
    }
}
