using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNightCycle : MonoBehaviour
{
    [SerializeField] bool isDay;
    [SerializeField] bool isRaining = false;
    [Header("Time Settings")]
    [Range(0f, 24f)]
    [SerializeField] float currentTime;
    [SerializeField] float timeSpeed = 1f;

    [Header("CurrentTime")]
    [SerializeField] string currentTimeString;

    [Header("Sun Settings")]
    [SerializeField] Light sunLight;
    [SerializeField] float sunPosition = 1;
    [SerializeField] float sunIntensity = 1;
    [SerializeField] AnimationCurve sunIntensityMultiplier;
    

    [Header("Sun Color (All gradients must have the same amount of color keys)")]
    [SerializeField] AnimationCurve sunColorCurve;
    [SerializeField] Gradient sunLightColorWhenRaining;
    [SerializeField] Gradient sunLightColorWhenNotRaining;
    [SerializeField] Gradient sunLightColor;
    
    [Header("Ambience Color(All gradients must have the same amount of color keys)")]
    [SerializeField] AnimationCurve ambientColorCurve;
    [SerializeField] Gradient ambientWhenRaining;
    [SerializeField] Gradient ambientWhenNotRaining;
    [SerializeField] Gradient ambientColor;

    [Header("Rain Settings")]
    [SerializeField] private WeatherManager weatherManager;
    [Range(0, 500)]
    [SerializeField] int rainIterationSteps;
    [Range(0, 4)]
    [SerializeField] float rainTransitionTime;

    // Start is called before the first frame update
    void Start()
    {
        CheckShadowStatus();
        weatherManager.OnRainStarts += OnRainStarts;
        weatherManager.OnRainEnds += OnRainEnds;
       
    }

    private void OnRainStarts()
    {
        isRaining = true;
        StartCoroutine(ChangeSunLightColor(sunLightColorWhenRaining, ambientWhenRaining));

    }

    private void OnRainEnds()
    {
        isRaining = false;
        StartCoroutine(ChangeSunLightColor(sunLightColorWhenNotRaining, ambientWhenNotRaining));
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

//se activan y desactivan sombras, tanto cuando llueve como cuando es de noche
    void CheckShadowStatus()
    {
        float currentSunRotation = currentTime;
         if (isRaining)
        {
            float lerpTime = 0;
            float lerpSpeed = 1f;
            lerpTime += Time.deltaTime*lerpSpeed;
            sunLight.shadowStrength = Mathf.Lerp(sunLight.shadowStrength, 0, lerpTime);
        }
        else if (currentSunRotation >= 5 && currentSunRotation <= 17)
        {
            float lerpTime = 0;
            float lerpSpeed = 0.5f;
            lerpTime += Time.deltaTime*lerpSpeed;
            sunLight.shadowStrength = Mathf.Lerp(sunLight.shadowStrength, 1, lerpTime);
            isDay = true;
        }
        else
        {
            float lerpTime = 0;
            float lerpSpeed = 1f;
            lerpTime += Time.deltaTime*lerpSpeed;
            sunLight.shadowStrength = Mathf.Lerp(sunLight.shadowStrength, 0, lerpTime);
            isDay = false;
        }
    }

    //se cambian los colores del sol y la luz ambiental cuando llueve

    IEnumerator ChangeSunLightColor(Gradient targetLight, Gradient targetAmbient)
    {
       
        Gradient originalGradientLight = new Gradient();
        originalGradientLight.SetKeys(sunLightColor.colorKeys, sunLightColor.alphaKeys);

        
        Gradient originalGradientAmbience = new Gradient();
        originalGradientAmbience.SetKeys(ambientColor.colorKeys, ambientColor.alphaKeys);

        
        for (int i = 0; i < rainIterationSteps; i++)
        {
          
            Gradient transitionGradientLight = new Gradient();
            Gradient transitionGradientAmbience = new Gradient();

           
            GradientColorKey[] colorKeysLight = new GradientColorKey[sunLightColor.colorKeys.Length];
            GradientColorKey[] colorKeysAmbience = new GradientColorKey[ambientColor.colorKeys.Length];

            
            for (int j = 0; j < sunLightColor.colorKeys.Length; j++)
            {
                Color startColor = originalGradientLight.colorKeys[j].color;
                Color targetColor = targetLight.colorKeys[j].color;
                Color lerpedColor = Color.Lerp(startColor, targetColor, ((float)i / (float)rainIterationSteps));
                colorKeysLight[j] = new GradientColorKey(lerpedColor, originalGradientLight.colorKeys[j].time);
            }
            transitionGradientLight.SetKeys(colorKeysLight, originalGradientLight.alphaKeys);

            
            sunLightColor = transitionGradientLight;

           
            for (int j = 0; j < ambientColor.colorKeys.Length; j++)
            {
                Color startColor = originalGradientAmbience.colorKeys[j].color;
                Color targetColor = targetAmbient.colorKeys[j].color;
                Color lerpedColor = Color.Lerp(startColor, targetColor, ((float)i / (float)rainIterationSteps));
                colorKeysAmbience[j] = new GradientColorKey(lerpedColor, originalGradientAmbience.colorKeys[j].time);
            }
            transitionGradientAmbience.SetKeys(colorKeysAmbience, originalGradientAmbience.alphaKeys);

            
            ambientColor = transitionGradientAmbience;

            
            yield return new WaitForSeconds(rainTransitionTime / rainIterationSteps);
        }
    }
}

 
