using DG.Tweening;
#if STEAM_ENABLED
using Steamworks;
#endif
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Dogabeey.SimpleJSON;
using Unity.VisualScripting;

namespace Dogabeey
{
    public class PlayerInputManager : MonoBehaviour, ISaveable
    {
        public static PlayerInputManager Instance;
        [Header("References")]
        public Entity playerEntity;
        public RectTransform pauseMenu;
        [Header("Settings")]
        public float moveStep;  // Speed at which the character moves
        public float moveCooldown;
        public float moveCooldownAcc;
        public float moveCooldownMin;
        public float moveCooldownResetCD;
        public LayerMask obstacleMask;
        public LayerMask shadowMask;
        public LayerMask carriableMask;
        [Header("Preferences")]
        public bool carriablesCantBePushedToShadows;

        internal InputControls controls;
        internal float defaulMoveCooldown;

        Vector3 direction;
        private bool movementOnCD;
        private Tween resetFillTween, exitFillTween;
        private Collider2D[] obstacleCols;
        private Transform playerTransform;
        private bool holdingAltInteract;
        private uint movementCount = 0;


#if STEAM_ENABLED
        protected Callback<UserStatsReceived_t> m_UserStatsReceived;
        protected Callback<UserStatsStored_t> m_UserStatsStored;
        protected Callback<UserAchievementStored_t> m_UserAchievementStored;
#endif

        public string SaveId => "PLAYER_INPUT";

        void Awake()
        {
            SaveManager.Instance.Register(this);

            Load();

            Instance = this;

            controls = new InputControls();
            controls.DefaultActionMap.Enable();

            defaulMoveCooldown = moveCooldown;
            playerTransform = playerEntity.transform;

            pauseMenu.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            controls.DefaultActionMap.Alt_Interact.performed += AltInteract_performed;
            controls.DefaultActionMap.Alt_Interact.canceled += AltInteract_canceled;
            controls.DefaultActionMap.Reset.started += Reset_started;
            controls.DefaultActionMap.Reset.canceled += Reset_canceled;
            controls.DefaultActionMap.Reset.performed += Reset_performed;
            controls.DefaultActionMap.ExitLevel.started += ExitLevel_started;
            controls.DefaultActionMap.ExitLevel.canceled += ExitLevel_canceled;
            controls.DefaultActionMap.ExitLevel.performed += ExitLevel_performed;
            controls.DefaultActionMap.Pause.performed += Cancel_performed;
        }

        private void OnLevelLocked(EventParam e)
        {
        }

        private void OnDisable()
        {
            controls.DefaultActionMap.Alt_Interact.performed -= AltInteract_performed;
            controls.DefaultActionMap.Alt_Interact.performed -= AltInteract_canceled;
            controls.DefaultActionMap.Reset.started -= Reset_started;
            controls.DefaultActionMap.Reset.canceled -= Reset_canceled;
            controls.DefaultActionMap.Reset.performed -= Reset_performed;
            controls.DefaultActionMap.ExitLevel.started -= ExitLevel_started;
            controls.DefaultActionMap.ExitLevel.canceled -= ExitLevel_canceled;
            controls.DefaultActionMap.ExitLevel.performed -= ExitLevel_performed;
            controls.DefaultActionMap.Pause.performed -= Cancel_performed;
        }

        private void AltInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            holdingAltInteract = true;
        }
        private void AltInteract_canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            holdingAltInteract = false;
        }
        private void Reset_started(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            // Fill reset image in 1 seconds using DOFloat
        }
        private void Reset_canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            resetFillTween.Kill();
        }
        private void ExitLevel_started(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            // Fill exit image in 1 seconds using DOFloat
        }
        private void ExitLevel_canceled(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            exitFillTween.Kill();
        }
        private void ExitLevel_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            WorldManager.Instance.EndCurrentLevel();
        }
        private void Reset_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            WorldManager.Instance.ResetCurrentLevel();
            // Pause all running tweens
        }
        private void Cancel_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            TogglePause();
        }

        public void TogglePause()
        {
            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
            //Time.timeScale = (pauseMenu.gameObject.activeSelf) ? 0 : 1;
            pauseMenu.gameObject.GetComponentInParent<Canvas>().sortingOrder = (pauseMenu.gameObject.activeSelf) ? 2 : 0;
            if(pauseMenu.gameObject.activeSelf)
            {
                playerEntity.movementLocked = true;
                DOTween.PauseAll();
            }
            else
            {
                playerEntity.movementLocked = false;
                DOTween.PlayAll();
            }
        }

        public void ExecuteMovement(Vector3 direction)
        {
            obstacleCols = GetCollidersInDirection(playerTransform, direction, obstacleMask);
            if (obstacleCols.Any())
            {
                return;
            }
            else
            {
                Collider2D[] shadows = GetCollidersInDirection(playerTransform, direction, shadowMask);
                List<Collider2D> cols = GetAllCarriables(playerTransform, direction, 5, out bool noPathAvailable);
                if (noPathAvailable) return;
                if (cols.Any())
                {
                    // If there is a shadow, but is not casted by that carriable, return.
                    if (shadows.Any() && shadows.Any(s => !s.transform.IsChildOrGrandchildOf(cols[0].transform)))
                    {
                        return;
                    }
                    else
                    {
                        foreach (Collider2D col in cols)
                        {
                            col.transform.DOBlendableMoveBy(direction, Const.Values.MOVEMENT_DURATION);
                        }
                        MoveDirection(playerTransform, direction);
                    }
                }
                else
                {
                    if (shadows.Any())
                    {
                        return;
                    }
                    MoveDirection(playerTransform, direction);
                }
            }
        }

        public bool CheckMovementInDirection(Vector3 direction)
        {
            Collider2D[] shadows = GetCollidersInDirection(playerTransform, direction, shadowMask);
            List<Collider2D> cols = GetAllCarriables(playerTransform, direction, 5, out bool noPathAvailable);
            if (noPathAvailable) return false;
            if (cols.Any())
            {
                // If there is a shadow, but is not casted by that carriable, return.
                if (shadows.Any() && shadows.Any(s => !s.transform.IsChildOrGrandchildOf(cols[0].transform)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (shadows.Any())
                {
                    return false;
                }
                return true;
            }
        }
        public bool CheckMovementInFourDirection()
        {
            return CheckMovementInDirection(moveStep * Vector3.right)
                || CheckMovementInDirection(moveStep * Vector3.left)
                || CheckMovementInDirection(moveStep * Vector3.up)
                || CheckMovementInDirection(moveStep * Vector3.down);
        }

        void Update()
        {
            if (playerEntity.movementLocked)
            {
                return;
            }
            if(movementOnCD)
            {
                return;
            }
            else
            {
                Vector2 axisVector = controls.DefaultActionMap.Move.ReadValue<Vector2>();

                // Enable up, down, left, right sprites based on movement direction.
                if (axisVector.x > 0)
                {
                    playerEntity.playerSprite.sprite = playerEntity.rightSprite;
                }
                if (axisVector.x < 0)
                {
                    playerEntity.playerSprite.sprite = playerEntity.leftSprite;
                }
                if (axisVector.y > 0)
                {
                    playerEntity.playerSprite.sprite = playerEntity.upSprite;
                } 
                if (axisVector.y < 0)
                {
                    playerEntity.playerSprite.sprite = playerEntity.downSprite;
                }

                playerEntity.lastPosition = playerTransform.position;

                if (Mathf.Abs(axisVector.x) > Mathf.Abs(axisVector.y)) // Left-Right 
                {
                    direction = Mathf.Sign(axisVector.x) * moveStep * Vector3.right;

                    ExecuteMovement(direction);
                }
                if (Mathf.Abs(axisVector.x) < Mathf.Abs(axisVector.y)) // Up-Down
                {
                    direction = Mathf.Sign(axisVector.y) * moveStep * Vector3.up;

                    ExecuteMovement(direction);
                }
                if(Mathf.Abs(axisVector.x) == Mathf.Abs(axisVector.y))
                {
                }

            }
        }

        // Get all carriable object in the direction of movement within an amount steps. Once finding obstacle, stop and return null. Once find null, return all carriable objects.
        private List<Collider2D> GetAllCarriables(Transform origin, Vector3 direction, int steps, out bool noPathAvailable)
        {
            List<Collider2D> carriables = new List<Collider2D>();

            for (int i = 0; i < steps; i++)
            {
                Collider2D[] tempCarriables = GetCollidersInDirection(origin, direction * (i + 1), carriableMask);
                Collider2D[] tempObstacles;

                if(carriablesCantBePushedToShadows)
                {
                    tempObstacles = GetCollidersInDirection(origin, direction * (i + 1), obstacleMask | shadowMask);
                }
                else
                {
                    tempObstacles = GetCollidersInDirection(origin, direction * (i + 1), obstacleMask);
                }

                if (tempCarriables.Any())
                {
                    carriables.Add(tempCarriables[0]);
                }
                else if(tempObstacles.Any())
                {
                    noPathAvailable = true;
                    return new List<Collider2D>();
                }
                else
                {
                    break;
                }
            }

            noPathAvailable = false;
            return carriables;
        }

        private Collider2D[] GetCollidersInDirection(Transform origin, Vector3 direction, LayerMask mask)
        {
            return Physics2D.OverlapCircleAll(origin.position + direction, Const.Values.MOVEMENT_OVERLAP_SPHERE_SENSITIVITY, mask, -100, 100);
        }

        private void MoveDirection(Transform movingObject, Vector3 direction)
        {
            movementOnCD = true;
            moveCooldown -= (moveCooldownMin <= moveCooldown) ? moveCooldownAcc * (SettingsManager.Instance.movementSensitivty + 0.5f) : 0;


            DOVirtual.DelayedCall(moveCooldown, 
                () =>
                {
                    movementOnCD = false;

                    if (!CheckMovementInFourDirection())
                    {
                        playerEntity.movementLocked = true;
                        EventManager.TriggerEvent(Const.GameEvents.LEVEL_LOCKED, new EventParam());
                    }
                }
            );
            playerEntity.moveTween = movingObject.DOBlendableMoveBy(direction, Const.Values.MOVEMENT_DURATION);
            //Round player position to closest int
            playerEntity.moveTween.OnComplete(() =>
            {
                Vector3 pos = movingObject.position;
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
                movingObject.position = pos;
            });

            // If holding AltInteract button, move all carriable objects at reverse direction in the direction of movement. (Pull them instead of push)
            if(holdingAltInteract)
            {
                Collider2D[] cols = GetCollidersInDirection(movingObject, -direction, carriableMask);
                foreach (Collider2D col in cols)
                {
                    col.transform.DOBlendableMoveBy(direction / 2, Const.Values.MOVEMENT_DURATION);
                }
            }
        }

        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> saveData = new Dictionary<string, object>
            {
                { "movementCount", movementCount }
            };

            return saveData;
        }
        public void Load()
        {
            JSONNode saveData = SaveManager.Instance.LoadSave(this);

            if (saveData == null)
            {
                return;
            }

            movementCount = (uint) saveData["movementCount"];
        }
    }
}
