using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.Events;


namespace Dogabeey
{
    public class Activatable : MonoBehaviour
    {
        public List<Activator> registeredActivators = new List<Activator>();
        public UnityEvent onActivate;
        public UnityEvent onDeactivate;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnActivate()
        {
            if (registeredActivators.TrueForAll(a => a.isActivated))
            {
                onActivate.Invoke();
            }
        }
        public void OnDeactivate()
        {
            onDeactivate.Invoke();
        }
    }
}