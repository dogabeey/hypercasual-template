using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dogabeey
{
    public class PressurePlateActivator : Activator
    {
        bool isPressed = false;

        public override bool CanActivate()
        {
            return isPressed;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            isPressed = true;
        }
        protected void OnTriggerStay2D(Collider2D collision)
        {
            isPressed = true;
        }
        protected void OnTriggerExit2D(Collider2D collision)
        {
            isPressed = false;
        }
    }
}