using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
   

    void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    }

    
    public void LoadGame()
    { 
        loadingScreen.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync("MainMenu"));
        scenesLoading.Add(SceneManager.LoadSceneAsync("MainGame", LoadSceneMode.Additive));


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
        SceneManager.UnloadSceneAsync("PersistentScene");
        
    }
    

}
