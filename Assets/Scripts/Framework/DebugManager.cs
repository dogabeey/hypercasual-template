using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class DebugManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_COMPLETED);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                EventManager.TriggerEvent(Const.GameEvents.LEVEL_FAILED);
            }
        }
    }
}
