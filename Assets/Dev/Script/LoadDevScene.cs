using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDevScene : MonoBehaviour
{
   
    void Start()
    {
        SceneManager.LoadSceneAsync("DevMainGame", LoadSceneMode.Additive);
    }

}
