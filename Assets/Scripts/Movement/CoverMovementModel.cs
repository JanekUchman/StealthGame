using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Movement
{
    public class CoverMovementModel : MovementModel
    {
        public CoverMovementModel(float walkMoveSpeed, float sprintMoveSpeed, float crouchMoveSpeed) : base(walkMoveSpeed, sprintMoveSpeed, crouchMoveSpeed)
        {
        }

        public override Vector2 GetMovement(Vector2 input)
        {
            var movement = input;
            movement.x = 0;
            return movement * moveSpeed;
        }
    }
}
