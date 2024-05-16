using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public class PlayButton : MenuButton
    {
        public WorldManagerUI worldListScreen;
        protected override void Start()
        {
            base.Start();
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            ScreenManager.Instance.Show(Const.Screens.WorldList);
        }
    }
}
