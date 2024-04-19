
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuUI : MonoBehaviour
{
    
    
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] Animator animCutOff;

    
    public static bool canChangeScene;

    void Start ()
    {
        
        AudioManager.instance.PlayMusic(FMODEvents.instance.menuMusic);
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!canChangeScene) return;
            AudioManager.instance.SetMenuMusicEnd();
            animCutOff.Play("RTransitionImgAnim");
            LeanTween.delayedCall(2f, () => { 
            GameManager.instance.LoadGame();
            });
        }
    }

}
