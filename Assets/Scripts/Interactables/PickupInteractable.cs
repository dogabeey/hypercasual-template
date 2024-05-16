using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class PickupInteractable : InteractableObject
    {
        public LayerMask shadowLayer;
        public Color pickedColor = Color.gray;
        public ParticleSystem possessParticle;

        internal new Collider2D collider2D;
        internal new Rigidbody2D rigidbody2D;
        internal Transform defaultParent;
        internal Color defaultColor;
        internal int defaultLayer;

        private float collisionToggleCD = 0.25f;
        private bool collisionOnCD = false;
        private bool isColliding = false;

        private void Start()
        {
            collider2D = GetComponent<Collider2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            defaultLayer = gameObject.layer;

            defaultParent = transform.parent;
        }

        public override void OnPlayerInteract(Entity entity)
        {
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If collided with shadow object
            if(shadowLayer == (shadowLayer | (1 << collision.gameObject.layer)))
            {
                if (!collision.transform.IsChildOf(transform))
                {
                    collisionOnCD = true;
                    isColliding = true;
                    DOVirtual.DelayedCall(collisionToggleCD, () => collisionOnCD = false).OnComplete(() =>
                    {
                        if(isColliding)
                        {
                            UnfreezeMovement();
                        }
                    });
                    rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            // If collided with shadow object
            if (shadowLayer == (shadowLayer | (1 << collision.gameObject.layer)))
            {
                if (!collision.transform.IsChildOf(transform))
                {
                    isColliding = false;
                    if (!collisionOnCD)
                    {
                        UnfreezeMovement();
                    }
                }
            }
        }

        private void UnfreezeMovement()
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}