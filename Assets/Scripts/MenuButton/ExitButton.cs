using Dogabeey.SimpleJSON;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey

{
    public class ExitButton : MenuButton
    {
        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            Application.Quit();
        }
    }
}
