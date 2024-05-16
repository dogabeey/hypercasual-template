using Dogabeey.SimpleJSON;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dogabeey

{
    public class ContinueButton : MenuButton
    {
        public string worldSaveID = "World_Manager";
        public override bool IsActive()
        {
            if (WorldManager.Instance.MainWorld.lastPlayedLevelIndex > 0)
            {
                return true;
            }
            else
            { 
                return false; 
            }
        }

        public override void OnClick()
        {
            WorldManager.Instance.CurrentWorld = WorldManager.Instance.worlds.FirstOrDefault(w => w.mainWorld);
            WorldManager.Instance.LoadCurrentLevel();
        }
    }
}
