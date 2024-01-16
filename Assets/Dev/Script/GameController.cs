using UnityEngine;
using System.Runtime.InteropServices;

public class GameController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameOver();

    public void SomeMethod()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameOver ();
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) SomeMethod();
    }
}
