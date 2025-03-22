using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multi2D
{
    public partial class LocalInputReceiver : IInputReceiver, IDisposable
    {
        private readonly LocalMultiplayerInput input;
        private IInputListener inputListener;

        public LocalInputReceiver(LocalMultiplayerInput input)
        {
            this.input = input;

            input.PlayerControll.Move.performed += OnMove;
            input.PlayerControll.Move.canceled += OnMove;

            input.PlayerControll.Jump.performed += OnJump;
            input.PlayerControll.Jump.canceled += OnJump;
            
            input.PlayerControll.Attack.performed += OnAttack;
            input.PlayerControll.Attack.canceled += OnAttack;
        }

        public void Enable() => input.Enable();
        public void Disable() => input.Disable();

        public void SetListener(IInputListener listener)
        {
            inputListener = listener ?? new MockInputListener();
        }

        public void Dispose()
        {
            Disable();

            input.PlayerControll.Move.performed -= OnMove;
            input.PlayerControll.Move.canceled -= OnMove;

            input.PlayerControll.Jump.performed -= OnJump;
            input.PlayerControll.Jump.canceled -= OnJump;

            input.PlayerControll.Attack.performed -= OnAttack;
            input.PlayerControll.Attack.canceled -= OnAttack;
        }

        private void OnMove(InputAction.CallbackContext ctx) 
            => inputListener.MovePerformed(ctx.ReadValue<Vector2>());

        private void OnJump(InputAction.CallbackContext context) 
            => inputListener.JumpPerformed(context.performed);

        private void OnAttack(InputAction.CallbackContext context) 
            => inputListener.FirePerformed(context.performed);
    }
}
