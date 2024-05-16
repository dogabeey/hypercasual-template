using Dogabeey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Dogabeey
{
    public class PositionConstraintPlayerSetter : MonoBehaviour
    {
        public PositionConstraint positionConstraint;

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.PLAYER_CREATED, OnPlayerCreated);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.PLAYER_CREATED, OnPlayerCreated);
        }

        private void Start()
        {
        }

        private void OnPlayerCreated(EventParam e)
        {
            GameObject player = GameObject.FindGameObjectWithTag(Const.TAGS.PLAYER);
            if (e.paramObj.TryGetComponent(out Entity entity))
            {
                ConstraintSource source = new ConstraintSource();
                source.sourceTransform = entity.transform;
                source.weight = 1;
                positionConstraint.SetSource(0, source);
                positionConstraint.constraintActive = true;
            }
        }
    }
}
