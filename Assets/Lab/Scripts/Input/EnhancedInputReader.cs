using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Multi2D.Input
{
    /// <summary>
    /// Enhanced input reader with buffering capabilities for improved responsiveness
    /// </summary>
    public class EnhancedInputReader : IInputReader, IDisposable
    {
        private readonly LocalMultiplayerInput input;
        private readonly InputBuffer inputBuffer;
        
        private Vector2 direction;
        private bool jumpPerformed;
        private bool attackPerformed;
        private float jumpPerformedTime;
        private float attackPerformedTime;

        private FrameInput frameInput;

        // Buffer configuration
        private const float JUMP_BUFFER_DURATION = 0.2f;
        private const float ATTACK_BUFFER_DURATION = 0.1f;

        public FrameInput FrameInput => frameInput;

        public EnhancedInputReader(LocalMultiplayerInput input)
        {
            this.input = input;
            this.inputBuffer = new InputBuffer();

            // Register buffered actions
            inputBuffer.RegisterBuffer(InputActionType.Jump, JUMP_BUFFER_DURATION);
            inputBuffer.RegisterBuffer(InputActionType.Attack, ATTACK_BUFFER_DURATION);

            SetupInputCallbacks();
        }

        private void SetupInputCallbacks()
        {
            input.PlayerControll.Move.performed += OnMove;
            input.PlayerControll.Move.canceled += OnMove;

            input.PlayerControll.Jump.performed += OnJump;
            input.PlayerControll.Jump.canceled += OnJump;
            
            input.PlayerControll.Attack.performed += OnAttack;
            input.PlayerControll.Attack.canceled += OnAttack;
        }

        public void UpdateFrameInput()
        {
            var currentTime = Time.time;
            
            // Check buffered inputs and consume them if available
            bool bufferedJump = inputBuffer.IsActionAvailable(InputActionType.Jump, currentTime);
            bool bufferedAttack = inputBuffer.IsActionAvailable(InputActionType.Attack, currentTime);

            // Consume buffered actions
            if (bufferedJump)
            {
                inputBuffer.ConsumeAction(InputActionType.Jump, currentTime);
                jumpPerformed = true;
                jumpPerformedTime = currentTime;
            }

            if (bufferedAttack)
            {
                inputBuffer.ConsumeAction(InputActionType.Attack, currentTime);
                attackPerformed = true;
                attackPerformedTime = currentTime;
            }

            frameInput = new FrameInput
            {
                Direction = direction,
                JumpPerformed = jumpPerformed,
                AttackPerformed = attackPerformed,
                JumpPerformedTime = jumpPerformedTime,
                AttackPerformedTime = attackPerformedTime
            };
        }

        public void Enable() => input.Enable();
        
        public void Disable()
        {
            input.Disable();
            ResetJumpInput();
            ResetAttackInput();
            direction = Vector2.zero;
        }

        public void Dispose()
        {
            inputBuffer.Dispose();

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
            inputBuffer.ClearBuffer(InputActionType.Jump);
        }

        public void ResetAttackInput()
        {
            attackPerformed = false;
            attackPerformedTime = 0;
            inputBuffer.ClearBuffer(InputActionType.Attack);
        }

        private void OnMove(InputAction.CallbackContext ctx) 
        {
            direction = ctx.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Record jump input in buffer
                inputBuffer.RecordInput(InputActionType.Jump, Time.time);
                jumpPerformed = true;
                jumpPerformedTime = Time.time;
            }
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Record attack input in buffer
                inputBuffer.RecordInput(InputActionType.Attack, Time.time);
                attackPerformed = true;
                attackPerformedTime = Time.time;
            }
        }

        /// <summary>
        /// Update the input buffer (call this in Update or FixedUpdate)
        /// </summary>
        public void UpdateBuffer(float deltaTime)
        {
            inputBuffer.Update(deltaTime);
        }

        /// <summary>
        /// Check if a buffered action is available without consuming it
        /// </summary>
        public bool IsActionBuffered(InputActionType actionType)
        {
            return inputBuffer.IsActionAvailable(actionType, Time.time);
        }
    }
}