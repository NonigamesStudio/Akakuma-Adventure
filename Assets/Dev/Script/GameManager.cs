using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Action OnSceneChange;
    public static GameManager instance;
    public IPlayerData currentPlayerData;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] Slider loadingBar;
    [SerializeField] Camera managerCamera;
    float totalSceneProgress;
    private static List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    bool isLoading=false;
    [SerializeField] List <InventorySOPair> inventories;

    public enum scenes
    {
        MainMenu,
        DevIsla2,
        ArtIsla2,
        DevIsla1,
        ArtIsla1
    }
   

    void Awake()
    {
        instance = this;
        if (ItsOnlySceneLoaded())
        {
            SceneManager.LoadSceneAsync(scenes.MainMenu.ToString(), LoadSceneMode.Additive);
        }

        currentPlayerData=new NullPlayerData();
        foreach(InventorySOPair pair in inventories)
        {
            pair.inventoryRutimeBackup.CopyItemsFromSO(pair.inventoryStart);
        }
        
    }
    

    public void ChangeScenes(scenes sceneToSetActive, List<scenes> scenesLoad, List<scenes> scenesUnload=null)
    {

        OnSceneChange?.Invoke();

        if (isLoading) return;
        isLoading=true;
        
        if (scenesUnload!=null)
        {
            foreach (scenes sceneId in scenesUnload)
            {
                scenesLoading.Add(SceneManager.UnloadSceneAsync(sceneId.ToString()));
            }
        }else
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (i!=0 && scene.isLoaded) 
                { 
                    
                    scenesLoading.Add(SceneManager.UnloadSceneAsync(scene.buildIndex)); // Usa el índice de construcción para descargar.
                }
            }
        }
        foreach (scenes sceneId in scenesLoad)
        {
            // Verifica si la escena ya está cargada antes de intentar cargarla.
            scenesLoading.Add(SceneManager.LoadSceneAsync(sceneId.ToString(), LoadSceneMode.Additive));
            
        }
        
        
        
        StartCoroutine(GetSceneLoadProgress(sceneToSetActive));


    }

    public IEnumerator GetSceneLoadProgress(scenes sceneToSetActive)
    {
        loadingScreen.SetActive(true);
        managerCamera.gameObject.SetActive(true);
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToSetActive.ToString()));

        if (currentPlayerData is NullPlayerData)
        {
           
        }
        else if (currentPlayerData is DataToTransfer)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                
                currentPlayerData.ApplyData(player);
                currentPlayerData=new NullPlayerData();
            }else 
            {
                Debug.LogError("Player not found");
            }
        }
        isLoading=false;

        
        
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

public interface IPlayerData
{
    public void ApplyData(Player player);
    
}

public class DataToTransfer: IPlayerData
{
    Health health;
    Vector3 position=Vector3.zero;
    

    public DataToTransfer(Player player)
    {
        if (player.TryGetComponent<Health>(out Health playerHealth))
        {
            this.health = playerHealth;
        }
    }
        
    public void ApplyData(Player player)
    {
        if (player.TryGetComponent<Health>(out Health playerHealth))
        {
            playerHealth.actualHealth=this.health.actualHealth;
            playerHealth.OnLifeChange?.Invoke(null);
        }
        if (position!=Vector3.zero)
        {
            player.transform.position=position;
        }
    }

    
}

public class NullPlayerData : IPlayerData
{
    public void ApplyData(Player player)
    {
        throw new NotImplementedException();
    }
}

[System.Serializable] class InventorySOPair
{
    public InventorySO inventoryStart;
    public InventorySO inventoryRutimeBackup;
}

