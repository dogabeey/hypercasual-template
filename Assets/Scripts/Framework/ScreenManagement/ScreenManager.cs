using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class ScreenManager : SingletonComponent<ScreenManager>
    {
        internal List<GameScreen> screens = new List<GameScreen>();

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            screens.AddRange(FindObjectsOfType<GameScreen>(true));

            //Show(firstScreen);
        }

        private void Update()
        {

        }

        public void Show(GameScreen gameScreen)
        {
            screens.ForEach(screen => screen.gameObject.SetActive(false));
            gameScreen.gameObject.SetActive(true);
        }
        public void Show(Screens screenID)
        {
            screens.ForEach(screen => screen.gameObject.SetActive(false));
            screens.Find(screen => screen.screenID == screenID).gameObject.SetActive(true);
        }
    }

}