using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace Dogabeey
{
    public class LevelScene : MonoBehaviour
    {
        [HideInInspector] public bool isWin;
        [HideInInspector] public bool isLose;
        [HideInInspector] public bool isEnded;

        public static LevelScene Instance;

        public string levelName;
        public RectTransform winPanel;
        public RectTransform losePanel;
        public Animator levelAnimator;

        private Camera tempCam;

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.LEVEL_COMPLETED, OnLevelCompleted);
            EventManager.StartListening(Const.GameEvents.LEVEL_FAILED, OnLevelFailed);
            EventManager.StartListening(Const.GameEvents.LEVEL_LOCKED, OnLevelLocked);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.LEVEL_COMPLETED, OnLevelCompleted);
            EventManager.StopListening(Const.GameEvents.LEVEL_FAILED, OnLevelFailed);
            EventManager.StopListening(Const.GameEvents.LEVEL_LOCKED, OnLevelLocked);
        }

        void OnLevelCompleted(EventParam e)
        {
            if (levelAnimator)
            {
                //Play animation and wait until the animation ends
                levelAnimator.SetTrigger("WinLevel");
                DOVirtual.DelayedCall(3, () =>
                {
                    ExecuteWinGame();

                });
            }
            else
            {
                ExecuteWinGame();
            }
        }
        void OnLevelFailed(EventParam e)
        {
            if (losePanel)
            {
                losePanel.gameObject.SetActive(true);
            }
            else
            {
                WorldManager.Instance.ResetCurrentLevel();
            }
        }
        void OnLevelLocked(EventParam e)
        {
            PreLose();
        }
        void ExecuteWinGame()
        {
            World.Instance.CurrentLevel.gameObject.SetActive(false);
            PlayerInputManager.Instance.gameObject.SetActive(false);
            if (winPanel)
            {
                winPanel.gameObject.SetActive(true);
            }
            else
            {
                WorldManager.Instance.LoadNextLevel();
            }
        }
        void PreLose()
        {
            Debug.Log("You are stuck!");
            PlayerInputManager.Instance.gameObject.SetActive(false);

            if (levelAnimator)
            {
                //Play animation and wait until the animation ends
                levelAnimator.SetTrigger("LoseLevel");
                DOVirtual.DelayedCall(3, () =>
                {
                    EventManager.TriggerEvent(Const.GameEvents.LEVEL_FAILED, new EventParam());
                });
            }
            else
            {
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_FAILED, new EventParam());
            }
        }
        private void Awake()
        {
            Instance = this;
        }


        void Start()
        {
            GameObject tempCamObject = GameObject.FindGameObjectWithTag("TempCamera");
            if(tempCamObject)
            {
                if(tempCamObject.TryGetComponent(out Camera camera))
                {
                    camera.gameObject.SetActive(false);
                }
            }
            Camera.main.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if(tempCam)
            {
                tempCam.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (isEnded) return;
            if (isWin) // PUT YOUR WIN CONDITIONS HERE
            {
                isEnded = true;
                EventParam param = new EventParam();
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_COMPLETED, param); // You can trigger this event anywhere and It will trigger On Win actions in the inspector, along with regular Level Completion events. This one also passes the time it took to win the level.
            }
            if (isLose) // PUT YOUR LOSE CONDITIONS HERE
            {
                isEnded = true;
                EventParam param = new EventParam();
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_FAILED, param); // You can trigger this event anywhere and It will trigger It will trigger On Lose actions in the inspector, along with regular Level Failure events. This one also passes the time it took to lose the level.
            }


        }

    }
}