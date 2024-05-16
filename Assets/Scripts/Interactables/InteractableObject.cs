using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public abstract class InteractableObject : MonoBehaviour
    {
        public string playerTag;
        public string interactableName;
        public string interactString;

        internal BoxCollider2D boxCollider2D;

        private void Awake()
        {
            TryGetComponent(out boxCollider2D);
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag) && other.TryGetComponent(out Entity entity))
            {
                OnPlayerEnterRange(entity);
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag) && other.TryGetComponent(out Entity entity))
            {
                OnPlayerExitRange(entity);
            }
        }
        public virtual void OnPlayerEnterRange(Entity entity)
        {
            EventManager.TriggerEvent(Const.GameEvents.PLAYER_ENTERED_RANGE, new EventParam(paramObj: gameObject, entity: entity));
        }
        public virtual void OnPlayerExitRange(Entity entity)
        {
            EventManager.TriggerEvent(Const.GameEvents.PLAYER_EXITED_RANGE, new EventParam(paramObj: gameObject, entity: entity));
        }

        public abstract void OnPlayerInteract(Entity entity);
    }
}