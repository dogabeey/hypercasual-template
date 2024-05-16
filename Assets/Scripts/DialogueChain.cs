using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChain : MonoBehaviour
{
    public List<DialogueNode> dialogueNodes;
    public int currentNodeIndex = 0;
    public bool active = false;
    public bool startOnAwake = false;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public DialogueNode GetCurrentNode()
    {
        return dialogueNodes[currentNodeIndex];
    }

    public void Advance()
    {
        DialogueNode node = GetCurrentNode();
        currentNodeIndex++;
        if(currentNodeIndex >= dialogueNodes.Count)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            //currentNodeIndex = 0;
        }
    }

}
