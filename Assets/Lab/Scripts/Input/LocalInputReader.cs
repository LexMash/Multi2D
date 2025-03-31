﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multi2D
{
    public partial class LocalInputReader : IInputReader, IDisposable
    {
        private readonly LocalMultiplayerInput input;

        private Vector2 direction;
        private bool jumpPerformed;
        private bool attackPerformed;
        private float jumpPerformedTime;

        public FrameInput FrameInput { get; private set; }

        public LocalInputReader(LocalMultiplayerInput input)
        {
            this.input = input;

            input.PlayerControll.Move.performed += OnMove;
            input.PlayerControll.Move.canceled += OnMove;

            input.PlayerControll.Jump.performed += OnJump;
            input.PlayerControll.Jump.canceled += OnJump;
            
            input.PlayerControll.Attack.performed += OnAttack;
            input.PlayerControll.Attack.canceled += OnAttack;
        }

        public void UpdateFrameInput()
        {
            FrameInput = new FrameInput
            {
                Direction = direction,
                JumpPerformed = jumpPerformed,
                AttackPerformed = attackPerformed,
                JumpPerformedTime = jumpPerformedTime,
            };
        }

        public void Enable() => input.Enable();
        public void Disable()
        {
            input.Disable();

            ResetJumpInput();
            direction = Vector2.zero;
            attackPerformed = false;
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

        public void ResetJumpInput()
        {
            jumpPerformed = false;
            jumpPerformedTime = 0;
        }

        private void OnMove(InputAction.CallbackContext ctx) => direction = ctx.ReadValue<Vector2>();
        private void OnJump(InputAction.CallbackContext context)
        {
            jumpPerformed = context.performed;
            jumpPerformedTime = jumpPerformed ? Time.time : 0;
        }

        private void OnAttack(InputAction.CallbackContext context) => attackPerformed = context.performed;
    }
}
