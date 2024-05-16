namespace Dogabeey
{
    public class ReturnToMenuButton : MenuButton
    {
        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            WorldManager.Instance.EndCurrentLevel();
        }
    }
}
