using DG.Tweening;
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{
    public class MenuInputManager : MonoBehaviour
    {
        public static MenuInputManager Instance;

        public UINode selectedNode;

        internal InputControls controls;

        public UINode SelectedNode { get => selectedNode;
            set
            {
                selectedNode = value;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            Button button;
            if (button = selectedNode.gameObject.GetComponentInChildren<Button>())
            {
                button.Select();
            }
        }

        public void Awake()
        {
            Instance = this;
            controls = new InputControls();
            controls.DefaultActionMap.Enable();
        }

        private void OnEnable()
        {
            controls.DefaultActionMap.Move.performed += ctx => TraverseLevelNodes(ctx.ReadValue<Vector2>());
            controls.DefaultActionMap.Interact.performed += Interact_performed;
            controls.DefaultActionMap.Jump.performed += Interact_performed;
        }
        private void OnDisable()
        {
            controls.DefaultActionMap.Move.performed -= ctx => TraverseLevelNodes(ctx.ReadValue<Vector2>());
            controls.DefaultActionMap.Interact.performed -= Interact_performed;
            controls.DefaultActionMap.Jump.performed -= Interact_performed;
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {

        }

        // Traverse the level nodes based on the direction of axisVector.
        public void TraverseLevelNodes(Vector2 axisVector)
        {
            if (axisVector.x > 0)
            {
                if (SelectedNode.rightNode != null)
                {
                    SelectedNode = SelectedNode.rightNode;
                }
            }
            else if (axisVector.x < 0)
            {
                if (SelectedNode.leftNode != null)
                {
                    SelectedNode = SelectedNode.leftNode;
                }
            }
            else if (axisVector.y > 0)
            {
                if (SelectedNode.upNode != null)
                {
                    SelectedNode = SelectedNode.upNode;
                }
            }
            else if (axisVector.y < 0)
            {
                if (SelectedNode.downNode != null)
                {
                    SelectedNode = SelectedNode.downNode;
                }
            }
        }   
    }

}
