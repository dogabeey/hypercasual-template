using Dreamteck.Splines;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


namespace Dogabeey
{
    public abstract class Activator : SerializedMonoBehaviour
    {
        public List<Activatable> outputs = new List<Activatable>();
        public bool isActivated = false;
        public string activateText, deactivateText;

        private Animator animator;
        private SplineComputer spline;
        private NavMeshAgent agent;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void Start()
        {

            foreach (Activatable output in outputs)
            {
                output.registeredActivators.Add(this);
                //AddSpline(output);
            }
        }

        private void AddSpline(Activatable item)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(agent.transform.position, item.transform.position, NavMesh.AllAreas, path))
            {
                SplinePoint[] points = new SplinePoint[path.corners.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new SplinePoint(path.corners[i]);
                }

                spline.SetPoints(points);
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (CanActivate() && !isActivated)
            {
                if (animator) animator.SetTrigger(activateText);
                OnActivate();
            }
            if (!CanActivate() && isActivated)
            {
                if (animator) animator.SetTrigger(deactivateText);
                OnDeactivate();
            }
        }

        public abstract bool CanActivate();

        public virtual void OnActivate()
        {
            isActivated = true;
            foreach (Activatable output in outputs)
            {
                output.OnActivate();
            }
        }
        public virtual void OnDeactivate()
        {
            isActivated = false;
            foreach (Activatable output in outputs)
            {
                output.OnDeactivate();
            }
        }
    }
}