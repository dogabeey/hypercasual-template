using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class LightSensorActivator : Activator
    {
        public List<Transform> cornerPoints;
        public Transform lightSource;
        public LayerMask raycastLayer;

        private bool isExposed;

        private bool IsExposedToLight()
        {
            foreach (Transform corner in cornerPoints)
            {
                Debug.DrawRay(corner.position, lightSource.position - corner.position, Color.red, 1);
                if (Physics.Raycast(corner.position, lightSource.position - corner.position, out RaycastHit hit, 100f, raycastLayer))
                {
                    if (hit.transform == lightSource)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Start()
        {
            InvokeRepeating(nameof(CheckExposed), 0, 0.5f);
        }

        private void CheckExposed()
        {
            isExposed = IsExposedToLight();
        }

        public override bool CanActivate()
        {
            return isExposed;
        }
    }
}