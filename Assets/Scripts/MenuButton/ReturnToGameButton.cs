namespace Dogabeey
{
    public class ReturnToGameButton : MenuButton
    {
        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            PlayerInputManager.Instance.TogglePause();
        }
    }
}
