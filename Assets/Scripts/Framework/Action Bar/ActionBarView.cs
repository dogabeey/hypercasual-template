using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    /// <summary>
    /// Action Bar View draws the action bar item's properties into UI elements.
    /// </summary>
    public class ActionBarView : MonoBehaviour
    {
        public ActionBarItem actionBarItem;
        public Image actionBarIcon;
        public TMP_Text actionText;
        public TMP_Text levelText;
        public TMP_Text costText;
        public Button onClickButton;
        public CanvasGroup canvasGroup;

        private void Start()
        {
            
        }

        public void DrawUI()
        {
            if (actionBarItem != null)
            {
                actionBarIcon.sprite = actionBarItem.actionBarIcon;
                actionText.text = actionBarItem.actionName;
                levelText.text = actionBarItem.CurrentLevel.ToString();
                costText.text = actionBarItem.GetCost().ToString();

                onClickButton.interactable = actionBarItem.isClickable;
                canvasGroup.alpha = actionBarItem.isVisible ? 1 : 0;
            }
        }
    }
}