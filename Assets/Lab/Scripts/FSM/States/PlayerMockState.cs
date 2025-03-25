﻿using UnityEngine;

namespace Multi2D
{
    public class PlayerMockState : PlayerFsmStateBase
    {
        public override void Enter() => Debug.Log("Enter in mock state");
        public override void Exit() => Debug.Log("Exit from mock state");
    }
}
