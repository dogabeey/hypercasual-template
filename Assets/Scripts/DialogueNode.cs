using System;
using UnityEngine.Events;

[Serializable]
public class DialogueNode
{
    public string dialogueOwner;
    public string dialogueText;
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;
}
