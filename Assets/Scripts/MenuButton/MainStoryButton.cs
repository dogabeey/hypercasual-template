using DG.Tweening;
using System.Linq;

namespace Dogabeey
{
    public class MainStoryButton : MenuButton
    {

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
            WorldManager.Instance.CurrentWorld = WorldManager.Instance.worlds.FirstOrDefault(w => w.mainWorld);
            WorldManager.Instance.LoadLevel(WorldManager.Instance.CurrentWorld.levelScenes[0]);
            DOVirtual.DelayedCall(0.1f, () => World.Instance.lastPlayedLevelIndex = 0);
        }
    }
}
