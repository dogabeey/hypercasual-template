using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Dogabeey
{
	[RequireComponent(typeof(Entity))]
	[RequireComponent(typeof(Animator))]
	public class EntityAnimator : MonoBehaviour
	{
		[System.Serializable]
		public class BlendSetting
		{
			public string blendTreeName;
			public float value;
		}

		public List<BlendSetting> blendSettings;
		public float minVelocity = 0.01f;
		public string idleText;
		public string runText;
		public string jumpText;
		public string carryText;
		public string verticalText;
		public string horizontalText;

		public bool Idle
		{
            get
			{
				return entity.state == Entity.EntityState.Idle;
            }
        }
        public bool Run
        {
            get
            {
                return entity.state == Entity.EntityState.Run;
            }
        }
        public bool Jump
        {
            get
            {
				return Mathf.Abs(entity.rb.velocity.y) > minVelocity;
            }
        }
        public bool Carry
        {
            get
            {
				return entity.PickedObject != null;
            }
        }

        protected Animator animator;
		protected Entity entity;

		#region Events

		// Set trigger events here using the event listeners.
		public virtual void OnEnable()
		{
			EventManager.StartListening(Const.GameEvents.CREATURE_DEATH, OnDeath);
			EventManager.StartListening(Const.GameEvents.CREATURE_JUMP, OnJump);
		}
		public virtual void OnDisable()
		{
			EventManager.StopListening(Const.GameEvents.CREATURE_DEATH, OnDeath);
            EventManager.StopListening(Const.GameEvents.CREATURE_JUMP, OnJump);
        }
		void OnDeath(EventParam e)
		{
			if (e.paramObj.GetHashCode() == gameObject.GetHashCode())
			{
				animator.SetTrigger("die");
			}
		}
		void OnJump(EventParam e)
		{
			if (e.paramObj.GetHashCode() == gameObject.GetHashCode())
			{
				animator.SetTrigger("jump_trigger");
			}
		}

		#endregion

		public virtual void Start()
		{
			entity = GetComponent<Entity>();
			animator = GetComponent<Animator>();
		}

		public virtual void Update()
		{
			AnimationController();
			SetBlendValues();
		}
		/// <summary>
		/// A method that is executed every frame of update to control animation.
		/// </summary>
		public void AnimationController()
		{
			//animator.SetBool(idleText, Idle);
			//animator.SetBool(runText, Run);
			//animator.SetBool(jumpText, Jump);
			//animator.SetBool(carryText, Carry);


			//animator.SetFloat(verticalText, entity.rb.velocity.y);
			//animator.SetFloat(horizontalText, entity.rb.velocity.sqrMagnitude / entity.maxVelocity);
		}

		void SetBlendValues()
		{
			foreach (BlendSetting setting in blendSettings)
			{
				animator.SetFloat(setting.blendTreeName, setting.value);
			}
		}
	}
}