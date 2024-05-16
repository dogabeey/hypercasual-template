namespace Dogabeey
{
    public class NextLevelButton : MenuButton
    {
        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            WorldManager.Instance.LoadNextLevel();
        }
    }
}
