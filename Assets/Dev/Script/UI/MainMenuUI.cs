
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MainMenuUI : MonoBehaviour, SceneChanger
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
            ChangeScenes(GameManager.scenes.DevIsla2, new List<GameManager.scenes> {
                GameManager.scenes.DevIsla2,GameManager.scenes.ArtIsla2}/*, new List<GameManager.scenes> {
                GameManager.scenes.MainMenu}*/);
            });
        }
    }

    public void ChangeScenes(GameManager.scenes sceneToSetActive, List<GameManager.scenes> scenesLoad, List<GameManager.scenes> scenesUnload=null)
    {
        GameManager.instance.ChangeScenes(sceneToSetActive, scenesLoad, scenesUnload);
    }
}
