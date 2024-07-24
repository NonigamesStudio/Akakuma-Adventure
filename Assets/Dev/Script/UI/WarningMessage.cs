using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour
{
    public Button acceptButton;
    [SerializeField] Button cancellButton;
    [SerializeField] TMP_Text messageText;
    Player player;


    void OnEnable()
    {
        //player.RemoveStun(0f);
        player = FindObjectOfType<Player>();
        player.GetStuned(0);
        cancellButton.onClick.AddListener(Cancell);

    }
    void OnDisable()
    {
        
        cancellButton.onClick.RemoveAllListeners();
        acceptButton.onClick.RemoveAllListeners();
        SetText("-");
    }
    public void SetText(string message)
    {
        messageText.text = message;
    }

    public void Cancell()
    {
        player.RemoveStun(0.3f);
        gameObject.SetActive(false);
    }
}
