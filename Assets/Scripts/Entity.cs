using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class Entity : MonoBehaviour
    {
        public enum EntityState
        {
            Idle,
            Run,
            Carry,
            Dead
        }

        public bool isPlayer;
        public bool movementLocked;
        public EntityState state;
        public SpriteRenderer playerSprite;
        public LayerMask playerLayer;
        public LayerMask noShadowPlayerLayer;
        public LayerMask groundMask;
        public float moveSpeedMultiplier = 1;
        [Header("Sprites")]
        public Sprite upSprite;
        public Sprite downSprite;
        public Sprite leftSprite;
        public Sprite rightSprite;

        internal Rigidbody2D rb;
        internal Collider2D cd;
        internal bool isGrounded;
        internal List<PickupInteractable> pickupableObjects = new List<PickupInteractable>();
        internal Tween moveTween;
        internal Vector3 lastPosition;

        private PickupInteractable pickedObject;

        public PickupInteractable PickedObject
        {
            get
            {
                return pickedObject;
            }
            set
            {
                pickedObject = value;
            }
        }
        public float MoveSpeed
        {
            get
            {
                float finalMoveSpeed = moveSpeedMultiplier;

                return finalMoveSpeed;
            }
        }

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.PLAYER_ENTERED_RANGE, OnPlayerEnteredRange);
            EventManager.StartListening(Const.GameEvents.PLAYER_EXITED_RANGE, OnPlayerExitedRange);
            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (isPlayer)
                {
                    EventManager.TriggerEvent(Const.GameEvents.PLAYER_CREATED, new EventParam(paramObj: gameObject));
                }
            });
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.PLAYER_ENTERED_RANGE, OnPlayerEnteredRange);
            EventManager.StopListening(Const.GameEvents.PLAYER_EXITED_RANGE, OnPlayerExitedRange);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            moveTween.Kill();
            transform.DOMove(lastPosition, 0.06f).OnComplete(() =>
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);
                transform.position = pos;

            }
            );
        }

        private void OnPlayerEnteredRange(EventParam param)
        {
            pickupableObjects.Add(param.paramObj.GetComponent<PickupInteractable>());
        }
        private void OnPlayerExitedRange(EventParam param)
        {
            pickupableObjects.Remove(param.paramObj.GetComponent<PickupInteractable>());
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cd = GetComponent<Collider2D>();

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetMovementLock(bool value) => movementLocked = value;
    }
}