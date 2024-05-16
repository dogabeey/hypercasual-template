using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using DG.Tweening;

namespace Dogabeey
{
    public class InteractableUI : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TMP_Text interactableName;
        public TMP_Text interactText;

        InteractableObject interactableObject;
        
        InputControls controls;

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.PLAYER_ENTERED_RANGE, OnPlayerEnteredRange);
            EventManager.StartListening(Const.GameEvents.PLAYER_EXITED_RANGE, OnPlayerExitedRange);
            EventManager.StartListening(Const.GameEvents.PLAYER_PICKED_OBJECT, OnPlayerPickedObject);
            EventManager.StartListening(Const.GameEvents.PLAYER_DROPPED_OBJECT, OnPlayerDroppedObject);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.PLAYER_ENTERED_RANGE, OnPlayerEnteredRange);
            EventManager.StopListening(Const.GameEvents.PLAYER_EXITED_RANGE, OnPlayerExitedRange);
            EventManager.StopListening(Const.GameEvents.PLAYER_PICKED_OBJECT, OnPlayerPickedObject);
            EventManager.StopListening(Const.GameEvents.PLAYER_DROPPED_OBJECT, OnPlayerDroppedObject);
        }
        private void OnPlayerEnteredRange(EventParam param)
        {
            if(param.paramObj.GetComponent<InteractableObject>() == interactableObject)
            {
                canvasGroup.alpha = 1;
            }
        }
        private void OnPlayerExitedRange(EventParam param)
        {
            if (param.paramObj.GetComponent<InteractableObject>() == interactableObject)
            {
                canvasGroup.alpha = 0;
            }
        }
        private void OnPlayerPickedObject(EventParam param)
        {
            if (param.paramObj.GetComponent<InteractableObject>() == interactableObject)
            {
                DOVirtual.Float(1, 0, 0.35f, (float alpha) => { canvasGroup.alpha = alpha; }).SetEase(Ease.InElastic);
            }
        }
        private void OnPlayerDroppedObject(EventParam param)
        {
            if (param.paramObj.GetComponent<InteractableObject>() == interactableObject)
            {
                DOVirtual.Float(0, 1, 0.35f, (float alpha) => { canvasGroup.alpha = alpha; }).SetEase(Ease.InElastic);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            controls = new InputControls();
            controls.DefaultActionMap.Enable();

            interactableObject = GetComponentInParent<InteractableObject>();
            // Set Values
            interactableName.text = interactableObject.interactableName;
            interactText.text = controls.DefaultActionMap.Interact.bindings[0].effectivePath.Split('/').Last().ToUpper() + " to " + interactableObject.interactString;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
