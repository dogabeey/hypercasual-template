#if STEAM_ENABLED

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

namespace Dogabeey
{
    public class DogabeeySteamManager : SteamManager
    {
        [SerializeField] private bool removeAllAchievementsAtStart = false;
        protected override void OnEnable()
        {
            base.OnEnable();

            if (removeAllAchievementsAtStart)
            {
                SteamUserStats.ResetAllStats(true);
            }
            SteamFriends.SetRichPresence("steam_display", "#status");
        }
    }
}

#endif