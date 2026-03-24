using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D.Input
{
    /// <summary>
    /// Manages input buffering for actions like jumping to improve responsiveness
    /// </summary>
    public class InputBuffer : IDisposable
    {
        private readonly Dictionary<InputActionType, InputBufferEntry> bufferEntries = new();
        private readonly List<InputBufferEntry> activeBuffers = new();
        
        private float timeScale = 1f;

        public void SetTimeScale(float scale)
        {
            timeScale = Mathf.Max(0.001f, scale);
        }

        /// <summary>
        /// Register an input action for buffering
        /// </summary>
        /// <param name="actionType">Type of action to buffer</param>
        /// <param name="bufferDuration">How long to keep the input valid (in seconds)</param>
        public void RegisterBuffer(InputActionType actionType, float bufferDuration)
        {
            if (bufferEntries.ContainsKey(actionType))
            {
                bufferEntries[actionType].BufferDuration = bufferDuration;
                return;
            }

            var entry = new InputBufferEntry(actionType, bufferDuration);
            bufferEntries[actionType] = entry;
            activeBuffers.Add(entry);
        }

        /// <summary>
        /// Record an input action
        /// </summary>
        /// <param name="actionType">Type of action</param>
        /// <param name="inputTime">When the input occurred</param>
        public void RecordInput(InputActionType actionType, float inputTime)
        {
            if (!bufferEntries.TryGetValue(actionType, out var entry))
                return;

            entry.RecordInput(inputTime);
        }

        /// <summary>
        /// Check if an action is available in the buffer
        /// </summary>
        /// <param name="actionType">Type of action to check</param>
        /// <param name="currentTime">Current game time</param>
        /// <returns>True if action is available, false otherwise</returns>
        public bool IsActionAvailable(InputActionType actionType, float currentTime)
        {
            if (!bufferEntries.TryGetValue(actionType, out var entry))
                return false;

            return entry.IsAvailable(currentTime);
        }

        /// <summary>
        /// Consume an action from the buffer (marks it as used)
        /// </summary>
        /// <param name="actionType">Type of action to consume</param>
        /// <param name="currentTime">Current game time</param>
        /// <returns>True if action was consumed, false if not available</returns>
        public bool ConsumeAction(InputActionType actionType, float currentTime)
        {
            if (!bufferEntries.TryGetValue(actionType, out var entry))
                return false;

            return entry.Consume(currentTime);
        }

        /// <summary>
        /// Clear all buffered inputs for a specific action type
        /// </summary>
        /// <param name="actionType">Type of action to clear</param>
        public void ClearBuffer(InputActionType actionType)
        {
            if (bufferEntries.TryGetValue(actionType, out var entry))
            {
                entry.Clear();
            }
        }

        /// <summary>
        /// Clear all buffered inputs
        /// </summary>
        public void ClearAllBuffers()
        {
            foreach (var entry in bufferEntries.Values)
            {
                entry.Clear();
            }
        }

        /// <summary>
        /// Update buffer timers and clean up expired entries
        /// Call this once per frame
        /// </summary>
        /// <param name="deltaTime">Time since last update</param>
        public void Update(float deltaTime)
        {
            var currentTime = Time.time;
            
            for (int i = activeBuffers.Count - 1; i >= 0; i--)
            {
                var entry = activeBuffers[i];
                entry.Update(currentTime);

                // Remove expired entries from active list for performance
                if (entry.IsExpired(currentTime))
                {
                    activeBuffers.RemoveAt(i);
                }
            }
        }

        public void Dispose()
        {
            bufferEntries.Clear();
            activeBuffers.Clear();
        }
    }

    /// <summary>
    /// Represents a buffered input action
    /// </summary>
    public class InputBufferEntry
    {
        public InputActionType ActionType { get; }
        public float BufferDuration { get; set; }
        
        private float lastInputTime = -1f;
        private bool consumed = false;

        public InputBufferEntry(InputActionType actionType, float bufferDuration)
        {
            ActionType = actionType;
            BufferDuration = bufferDuration;
        }

        public void RecordInput(float inputTime)
        {
            lastInputTime = inputTime;
            consumed = false;
        }

        public bool IsAvailable(float currentTime)
        {
            if (consumed || lastInputTime < 0)
                return false;

            return (currentTime - lastInputTime) <= BufferDuration;
        }

        public bool Consume(float currentTime)
        {
            if (!IsAvailable(currentTime))
                return false;

            consumed = true;
            return true;
        }

        public void Clear()
        {
            lastInputTime = -1f;
            consumed = false;
        }

        public bool IsExpired(float currentTime)
        {
            return lastInputTime >= 0 && (currentTime - lastInputTime) > BufferDuration;
        }

        public void Update(float currentTime)
        {
            // Cleanup expired inputs
            if (IsExpired(currentTime))
            {
                Clear();
            }
        }
    }

    /// <summary>
    /// Types of input actions that can be buffered
    /// </summary>
    public enum InputActionType
    {
        Jump,
        Attack,
        Dash,
        SpecialMove
    }
}