
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    [SerializeField] Animator animCutOff;
    public static bool canChangeScene;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!canChangeScene) return;
            animCutOff.Play("RTransitionImgAnim");
            LeanTween.delayedCall(2f, () => { 
                SceneManager.LoadScene("ArtMainGame");
            });
        }
    }

}
