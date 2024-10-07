using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace P3
{
    public class PlayerInputReader : MonoBehaviour, InputSystem_Actions.IPlayerActions
    {
        public event Action AttackEvent;

        private InputSystem_Actions inputActions;

        private void Start()
        {
            inputActions=new InputSystem_Actions();

            inputActions.Player.SetCallbacks(this);  
        }

        private void OnEnable()
        {
              inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            AttackEvent?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}

