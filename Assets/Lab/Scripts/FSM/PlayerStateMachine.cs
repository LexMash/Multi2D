using Multi2D.FSM;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Multi2D
{
    public class PlayerStateMachine : IPlayerStateMachine, IPlayerStatesRegistrator, IDisposable
    {
        private readonly IChangeStateRequestedNotifier stateChangeRequestNotifier;
        private readonly Dictionary<Type, PlayerFsmStateBase> statesMap = new();

        private PlayerFsmStateBase currentState;

        public PlayerStateMachine(IChangeStateRequestedNotifier stateChangeRequestNotifier)
        {
            this.stateChangeRequestNotifier = stateChangeRequestNotifier;
        }

        public void Initialize()
        {
            stateChangeRequestNotifier.ChangeStateRequested += ChangeState;
            currentState ??= new PlayerMockState();
            currentState.Enter();
        }

        public void SetState<T>() where T : PlayerFsmStateBase 
            => ChangeState(typeof(T));

        public void Update(float deltaTime) 
            => currentState.Update(deltaTime);

        public IPlayerStatesRegistrator RegisterState<T>(T state, bool isInitial = false) where T : PlayerFsmStateBase
        {
            statesMap[typeof(T)] = state;

            if (isInitial)
                currentState = state;

            return this;
        }

        public void Dispose()
        {
            stateChangeRequestNotifier.ChangeStateRequested -= ChangeState;

            currentState?.Exit();
            currentState = null;
            statesMap.Clear();
        }

        private void ChangeState(Type type)
        {
            currentState?.Exit();

            if (!statesMap.TryGetValue(type, out PlayerFsmStateBase state))
            {
                state = new PlayerMockState();
                Debug.LogWarning($"{nameof(PlayerStateMachine)} doesn't contain state with type {type}");
            }            

            currentState = state;
            currentState.Enter();
        }
    }
}
