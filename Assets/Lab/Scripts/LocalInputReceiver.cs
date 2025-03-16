using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multi2D
{
    public class LocalInputReceiver : IDisposable, IInputReceiver
    {
        private readonly LocalMultiplayerInput input;

        public event Action<Vector2> MovePerformed = delegate { };
        public event Action JumpPerformed = delegate { };
        public event Action AttackPerformed = delegate { };

        public LocalInputReceiver(LocalMultiplayerInput input)
        {
            this.input = input;

            input.PlayerControll.Move.performed += OnMove;
            input.PlayerControll.Move.canceled += OnMove;
            input.PlayerControll.Jump.performed += OnJump;
            input.PlayerControll.Attack.performed += OnAttack;
        }

        public void Enable() => input.Enable();
        public void Disable() => input.Disable();

        public void Dispose()
        {
            Disable();

            input.PlayerControll.Move.performed -= OnMove;
            input.PlayerControll.Move.canceled -= OnMove;
            input.PlayerControll.Jump.performed -= OnJump;
        }

        private void OnMove(InputAction.CallbackContext ctx) => MovePerformed.Invoke(ctx.ReadValue<Vector2>());
        private void OnJump(InputAction.CallbackContext context) => JumpPerformed.Invoke();
        private void OnAttack(InputAction.CallbackContext context) => AttackPerformed?.Invoke();
    }
}
