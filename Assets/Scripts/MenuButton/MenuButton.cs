using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public abstract class MenuButton : MonoBehaviour
    {
        private Button actionButton;

        public abstract bool IsActive();
        public abstract void OnClick();

        protected virtual void Start()
        {
            actionButton = GetComponentInChildren<Button>();
            actionButton.onClick.AddListener(() => OnClick());
        }
        protected virtual void Update()
        {
            actionButton.interactable = IsActive(); 
        }
    }
}
