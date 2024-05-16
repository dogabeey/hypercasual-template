using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public class NewGameButton : MenuButton
    {
        public RectTransform newGamePrompt;
        public Button yesButton;
        public Button noButton;

        protected override void Start()
        {
            base.Start();
            yesButton.onClick.AddListener(OnYesClick);
            noButton.onClick.AddListener(OnNoClick);
        }

        public void OnYesClick()
        {
            WorldManager.Instance.LoadLevel(World.Instance.levelScenes[0]);
        }
        public void OnNoClick()
        {
            newGamePrompt.gameObject.SetActive(false);
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            if (World.Instance.lastPlayedLevelIndex != 0)
            {
                newGamePrompt.gameObject.SetActive(true);
            }
            else
            {
                WorldManager.Instance.LoadLevel(World.Instance.levelScenes[0]);

            }
        }
    }
}
