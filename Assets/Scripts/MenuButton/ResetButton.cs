using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class ResetButton : MenuButton
    {
        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            WorldManager.Instance.ResetCurrentLevel();
        }
    }
}
