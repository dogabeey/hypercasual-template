namespace Dogabeey
{
    public class LeverActivator : Activator
    {
        bool isOn = false;

        public override bool CanActivate()
        {
            return isOn;
        }

        public void Toggle()
        {
            isOn = !isOn;
        }
    }
}