//using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Cinemachine;
using UnityEditor;

public class DialogueTest : MonoBehaviour
{
    public static DialogueTest instance;

    public static Action OnStartDialogue;
    public static Action OnEndDialogue;

    [SerializeField] DialogueScriptable dialogueScriptable;
    [SerializeField] List<SpriteByIdData> spriteDic;
    [SerializeField] Player player;

    [Space(10)]
    [Header("Refs UI")]
    [SerializeField] GameObject panelDialogue;
    [SerializeField] Image spriteDialogue;
    [SerializeField] Image spriteDialoguePlayer;
    [SerializeField] TextMeshProUGUI conversation_txt;
    [Space(10)]
    [Header("Refs Camera")]
    [SerializeField] Animator cameraAnimator;
    [SerializeField] CinemachineVirtualCamera dialogueCamera;
    CinemachineOrbitalTransposer transposer;
    [SerializeField] CinemachineTargetGroup targetGroup;
    public bool isDialogueOpen;


    int indexLastTalked;


    bool ct;

    Coroutine coroutine;



    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;

        transposer = dialogueCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Start()
    {
        //dialogueScriptable = Resources.Load<DialogueScriptable>("DialogueScriptable");
        //dialogueScriptable.dialogueDataList = new List<DialogueData>();
        //LoadDialogues();
    }

    [ContextMenu("Create Dialogue Data Scriptable")]
    void LoadDialogues() //Crea el ScriptableObject con la info de las conversaciones, esto va a servir para crear un scriptable por escena 
    {
        ConvertCsvFile("Assets/Resources/Dialogos.csv");
    }

    public void ConvertCsvFile(string path)
    {
        var csv = new List<string[]>();
        string[] lines = File.ReadAllLines(path);

        foreach (var item in lines)
        {
            csv.Add(item.Split(","));
        }

        for (int i = 0; i < csv.Count; i++) //Fila 
        {
            for (int j = 0; j < csv[i].Length; j++) // Columna
            {
                if (csv[i][j] == "") continue;
                if (csv[i][j] == "*")
                {
                    DialogueData dialogo = new DialogueData();
                    dialogo.id = int.Parse(csv[i + 1][j]);
                    dialogueScriptable.dialogueDataList.Add(dialogo);
                    i++;
                    break;
                }
                //Carga los dialogos al scriptableObject
                dialogueScriptable.dialogueDataList[dialogueScriptable.dialogueDataList.Count-1].dialogos.Add(new ConversationData(int.Parse(csv[i][j]), csv[i][j+1].ToString()));
                break;

            }
        }
    }

    private void Update()
    {
        if (!isDialogueOpen)return;
        if (Input.GetMouseButtonDown(0)) ct = true;
    }


    public void PlayDialogue(int Id, Transform npctransform)
    {
        OnStartDialogue?.Invoke();
        isDialogueOpen =true;
        targetGroup.AddMember(npctransform,1,2);

        ToggleDialoguePanelAnim(true);

        foreach (var item in dialogueScriptable.dialogueDataList)
        {
            if (item.id == Id) { StartCoroutine(PlayDialogue(item, npctransform)); return; }
            
        }
    }

    IEnumerator  PlayDialogue(DialogueData dialogue, Transform npctransform)
    {
        
        player.enabled = false; // cambiar esto
        cameraAnimator.Play("DialogueCamera"); //Cambia la camera a la camara de dialogo
        transposer.m_XAxis.Value = -20;

        yield return new WaitForSeconds(0.3f);
        
        int index = 0;

        
        while (index != dialogue.dialogos.Count)
        {
            ct = false;
            if (dialogue.dialogos[index].idOfWhoTalk != indexLastTalked)
            {
                if (dialogue.dialogos[index].idOfWhoTalk == 1) //1 es akakuma
                {
                    spriteDialoguePlayer.sprite = FindSpriteById(dialogue.dialogos[index].idOfWhoTalk);
                    spriteDialoguePlayer.color = new Color32(255, 255, 255, 255);
                }
                else
                {   
                    spriteDialogue.sprite = FindSpriteById(dialogue.dialogos[index].idOfWhoTalk);
                    spriteDialogue.color = new Color32(255, 255, 255, 255);
                }
            }//Busca por id el sprite que se va a mostrar en la UI
            
            ////yield return new WaitForSeconds(0.1f);
            //yield return new WaitWhile(() => !Input.GetMouseButtonDown(0));

            coroutine =  StartCoroutine(AnimMovingText(dialogue.dialogos[index].conversationString, ct));

            yield return new WaitForSeconds(0.1f);
            yield return new WaitWhile(()=> !Input.GetMouseButtonDown(0)); //Hace que el texto se vaya escribiendo solo

            if (coroutine != null) StopCoroutine(coroutine);
            conversation_txt.text = dialogue.dialogos[index].conversationString;

            index++; //Indice necesario para pasar a la siguiente conversacion
            yield return new WaitForSeconds(0.1f);
            yield return new WaitWhile(() => !Input.GetMouseButtonDown(0));

            /*await WaitToClickToCotinueDialogue(); *///espera el click y pasar al siguiente texto

            // es la aniacion de la camara para se mueva cuando hablan
            AnimCamera(dialogue, index);

        }

       
        //End of dialogue
        spriteDialogue.color = new Color32(0, 0, 0, 0);
        spriteDialoguePlayer.color = new Color32(0, 0, 0, 0);
        isDialogueOpen = false;
        player.enabled = true;// cambiar esto
        OnEndDialogue?.Invoke();
        yield return new WaitForSeconds(0.5f);
        cameraAnimator.Play("NormalCamera");
        targetGroup.RemoveMember(npctransform);
        ToggleDialoguePanelAnim(false);
    }

    private void AnimCamera(DialogueData dialogue, int index)
    {
        if (index > dialogue.dialogos.Count - 1) index = dialogue.dialogos.Count - 1;
        if (dialogue.dialogos[index].idOfWhoTalk != indexLastTalked)
        {
            if (transposer.m_XAxis.Value == -20) LeanTween.value(gameObject, -20, -80, 0.5f).setEaseOutQuint().setOnUpdate((float value) => { transposer.m_XAxis.Value = value; });
            else LeanTween.value(gameObject, -120, -20, 0.5f).setEaseOutQuint().setOnUpdate((float value) => { transposer.m_XAxis.Value = value; });
        }
    }


    Sprite FindSpriteById(int id)
    {
        indexLastTalked = id;

        foreach (var item in spriteDic)
        {
            if (item.id == id) return item.sprite;
        }
        return null;
    }

    void ToggleDialoguePanelAnim(bool toggle)
    {
        if (toggle)
            LeanTween.moveY(panelDialogue, 0, 0.5f).setEaseOutBack();
        else
            LeanTween.moveY(panelDialogue, -400, 0.5f).setEaseInBack();

    }

   IEnumerator AnimMovingText(string conversation,bool asd)
    {
        conversation_txt.text = "";
        foreach (char item in conversation)
        {
            conversation_txt.text += item;
            yield return new WaitForSeconds(0.05f);
        }
    }

}



[System.Serializable]
public class DialogueData
{
    public int id;
    public List<ConversationData> dialogos;

    public DialogueData()
    {
        dialogos = new List<ConversationData>();
    }
}

[System.Serializable]
public class ConversationData
{
    public int idOfWhoTalk;
    public string conversationString;

    public ConversationData(int idOfWhoTalk, string conversationString)
    {
        this.idOfWhoTalk = idOfWhoTalk;
        this.conversationString = conversationString;
    }
}

[System.Serializable]
public class SpriteByIdData
{
    public int id;
    public Sprite sprite;
}







