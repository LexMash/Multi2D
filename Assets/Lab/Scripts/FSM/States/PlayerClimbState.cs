using Multi2D.FSM;
using UnityEngine;

namespace Multi2D.States
{
    public class PlayerClimbState : PlayerFsmStateBase
    {
        private readonly PlayerFsmStateChangeRequester stateChangeRequester;

        public override void Enter()
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void AttackPerformed(bool active)
        {
            throw new System.NotImplementedException();
        }

        public void JumpPerformed(bool active)
        {
            throw new System.NotImplementedException();
        }

        public void MovePerformed(Vector2 normalizedDirection)
        {
            throw new System.NotImplementedException();
        }
    }
}
