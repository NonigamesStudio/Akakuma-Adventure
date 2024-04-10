
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            GameManager.instance.LoadGame();
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

    }

}
