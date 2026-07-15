using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableDialogue", menuName = "Scriptable Objects/ScriptableDialogue")]
public class ScriptableDialogue : ScriptableObject
{
    public DialogueLine[] DialogueLines;


}

[Serializable]
public class DialogueLine
{
    public string Title;
    public string Line;
}