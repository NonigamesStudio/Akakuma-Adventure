using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogueScriptable")]
public class DialogueScriptable:ScriptableObject
{
    public List<DialogueData> dialogueDataList = new List<DialogueData>();
}
