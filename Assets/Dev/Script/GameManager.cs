using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] Slider loadingBar;
    [SerializeField] Camera managerCamera;
    float totalSceneProgress;
    private static List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private enum scenes
    {
        MainMenu,
        DevIsla1,
        ArtIsla1
    }
   

    void Awake()
    {
        instance = this;
        if (ItsOnlySceneLoaded())
        {
            SceneManager.LoadSceneAsync(Enum.GetName(typeof(scenes),0), LoadSceneMode.Additive);
        }
        
    }

    
    public void LoadGame()
    { 
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(Enum.GetName(typeof(scenes),0)));
        scenesLoading.Add(SceneManager.LoadSceneAsync(Enum.GetName(typeof(scenes),1), LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync(Enum.GetName(typeof(scenes),2), LoadSceneMode.Additive));
        
        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                managerCamera.gameObject.SetActive(true);
                
                totalSceneProgress = 0;
                foreach (AsyncOperation scene in scenesLoading)
                {
                    totalSceneProgress += scene.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                loadingBar.value = totalSceneProgress;

                
                
                yield return null;
            }

        }
        loadingScreen.SetActive(false);
        managerCamera.gameObject.SetActive(false);
        scenesLoading.Clear();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Enum.GetName(typeof(scenes),1)));
        
        
    }
    public bool ItsOnlySceneLoaded()
    {
   
    if (SceneManager.sceneCount > 1)
        {
            return false;
        }else
        {
            return true;
        }
    }
  

}
