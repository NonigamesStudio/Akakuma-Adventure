//using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Threading.Tasks;
using Cinemachine;
using UnityEditor;

public class DialogueTest : MonoBehaviour
{
    public static DialogueTest instance;

    public DialogueScriptable dialogueScriptable;
    [SerializeField] List<SpriteByIdData> spriteDic;
    [SerializeField] Player player;

    [Space(10)]
    [Header("Refs UI")]
    [SerializeField] GameObject panelDialogue;
    [SerializeField] Image spriteDialogue;
    [SerializeField] TextMeshProUGUI conversation_txt;
    [Space(10)]
    [Header("Refs Camera")]
    [SerializeField] Animator cameraAnimator;
    [SerializeField] CinemachineVirtualCamera dialogueCamera;
    CinemachineOrbitalTransposer transposer;
    [SerializeField] CinemachineTargetGroup targetGroup;

    

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else instance = this;

        transposer = dialogueCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    [ContextMenu("Create Dialogue Data Scriptable")]
    void LoadDialogues() //Crea el ScriptableObject con la info de las conversaciones, esto va a servir para crear un scriptable por escena 
    {
        dialogueScriptable = (DialogueScriptable)ScriptableObject.CreateInstance<DialogueScriptable>();
        ConvertCsvFile("Assets/Resources/Dialogos.csv");
        AssetDatabase.CreateAsset(dialogueScriptable, "Assets/Dev/DialoguesData/DialogueData.asset");
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




    public void PlayDialogue(int Id, Transform npctransform)
    {
        targetGroup.AddMember(npctransform,1,2);

        ToggleDialoguePanelAnim(true);

        foreach (var item in dialogueScriptable.dialogueDataList)
        {
            if (item.id == Id) { PlayDialogue(item, npctransform); return; }
            
        }
    }

    async void  PlayDialogue(DialogueData dialogue, Transform npctransform)
    {
        player.enabled = false; // cambiar esto
        cameraAnimator.Play("DialogueCamera"); //Cambia la camera a la camara de dialogo
        transposer.m_XAxis.Value = -20;

        await Task.Delay(300);
        
        int index = 0;

        while(index != dialogue.dialogos.Count)
        {

            spriteDialogue.sprite = FindSpriteById(dialogue.dialogos[index].idOfWhoTalk); //Busca por id el sprite que se va a mostrar en la UI

            await AnimMovingText(dialogue.dialogos[index].conversationString); //Hace que el texto se vaya escribiendo solo

            index++; //Indice necesario para pasar a la siguiente conversacion

            await WaitToClickToCotinueDialogue(); //espera el click y pasar al siguiente texto

            //Esto no va aca, es la aniacion de la camara para se mueva cuando hablan
            if (transposer.m_XAxis.Value == -20) LeanTween.value(gameObject, -20, 160, 0.5f).setEaseOutQuint().setOnUpdate((float value) => { transposer.m_XAxis.Value = value; });
            else LeanTween.value(gameObject, 160, -20, 0.5f).setEaseOutQuint().setOnUpdate((float value) => { transposer.m_XAxis.Value = value; });
        }

        //End of dialogue
        player.enabled = true;// cambiar esto
        cameraAnimator.Play("NormalCamera");
        targetGroup.RemoveMember(npctransform);
        ToggleDialoguePanelAnim(false);
    }

     async Task WaitToClickToCotinueDialogue()
     {
        while (!Input.GetMouseButtonDown(0)) await Task.Yield();
     }

    Sprite FindSpriteById(int id)
    {
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

   async Task AnimMovingText(string conversation)
    {
        conversation_txt.text = "";
        foreach (char item in conversation)
        {
            conversation_txt.text += item;
            await Task.Delay (50);
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

public class DialogueScriptable:ScriptableObject
{
    public List<DialogueData> dialogueDataList = new List<DialogueData>();
}




