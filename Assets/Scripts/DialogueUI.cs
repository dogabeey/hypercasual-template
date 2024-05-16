using Dogabeey;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public DialogueChain dialogueChain;
    public float textSpeed = 0.1f;
    public TMP_Text ownerDisplay;
    public TMP_Text textDisplay;
    public Button continueButton;
    public Image continueIndicator;

    private InputControls controls;
    private bool canContinue = false;
    private bool spedUp = false;
    private bool holdingContinue = false;

    private void OnEnable()
    {
        controls.DefaultActionMap.Enable();
        controls.DefaultActionMap.Interact.started += Interact_performed;
        controls.DefaultActionMap.Jump.started += Interact_performed;
        controls.DefaultActionMap.Interact.canceled += Interact_canceled;
        controls.DefaultActionMap.Jump.canceled += Interact_canceled;
    }


    private void OnDisable()
    {
        controls.DefaultActionMap.Interact.started -= Interact_performed;
        controls.DefaultActionMap.Interact.canceled -= Interact_canceled;
        controls.DefaultActionMap.Jump.started -= Interact_performed;
        controls.DefaultActionMap.Jump.canceled -= Interact_canceled;
        controls.DefaultActionMap.Disable();

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        TryAdvance();
        holdingContinue = true;
    }
    private void Interact_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        holdingContinue = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        controls = new InputControls();
    }

    private void Start()
    {
        DrawInit();
        if (dialogueChain.startOnAwake)
        {
            StartDialogue();
        }
    }
    // Update is called once per frame
    void Update()
    {
        DrawUpdate();
    }

    void DrawInit()
    {
        textDisplay.text = "";
        if (ownerDisplay) ownerDisplay.text = dialogueChain.GetCurrentNode().dialogueOwner;
        if (continueButton) continueButton.onClick.AddListener(TryAdvance);
    }
    void DrawUpdate()
    {
        continueIndicator.enabled = canContinue;
    }

    public void StartDialogue()
    {
        if (dialogueChain != null)
        {
            if(dialogueChain.active)
            {
                spedUp = false;
                canContinue = false;
                StartCoroutine(TypeText(dialogueChain.GetCurrentNode().dialogueText));
            }
        }
    }

    IEnumerator TypeText(string text)
    {
        if (dialogueChain.GetCurrentNode().onDialogueStart != null) 
                dialogueChain.GetCurrentNode().onDialogueStart.Invoke();
        foreach (char letter in text.ToCharArray())
        {
            textDisplay.text += letter;
            if((holdingContinue || spedUp))
            {
                yield return new WaitForSeconds(textSpeed/ 10);
            }
            else
            {
                yield return new WaitForSeconds(textSpeed);
                SoundManager.Instance.Play(Const.SOUNDS.EFFECTS.TYPEWRITER);
            }
        }
        if (dialogueChain.GetCurrentNode().onDialogueEnd != null) dialogueChain.GetCurrentNode().onDialogueEnd.Invoke();
        canContinue = true;
    }

    void TryAdvance()
    {
        if (canContinue)
        {
            dialogueChain.Advance();
            textDisplay.text = "";
            StartDialogue();
        }
        else
        {
            spedUp = true;
        }
    }
}
