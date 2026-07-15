using UnityEngine;
using TMPro;
public class DialogueInteractable : Interactable
{
    public ScriptableDialogue dialogue;
    [Space]
    public GameObject dialogueCanvas;
    public TMP_Text dialogueTitle;
    public TMP_Text dialogueBubble;

    private int currentDialogueLine = -1;

    private void Start()
    {
        dialogueCanvas.SetActive(false);
    }
    public override void Interact ()
    {
        currentDialogueLine++;

        if (currentDialogueLine < dialogue.DialogueLines.Length)
        {
            dialogueCanvas.SetActive(true);
            dialogueTitle.text = dialogue.DialogueLines[currentDialogueLine].Title;
            dialogueBubble.text = dialogue.DialogueLines[currentDialogueLine].Line;
        }
        else if (currentDialogueLine >= dialogue.DialogueLines.Length)
        {
            dialogueCanvas.SetActive(false);
            currentDialogueLine = -1;
        }
    }
}