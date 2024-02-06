using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
   

    public void RestartLvl()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    [DllImport("__Internal")]
    private static extern void GameOver();

    public void OnEndGame()
    {

#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameOver ();
#endif
    }

}
