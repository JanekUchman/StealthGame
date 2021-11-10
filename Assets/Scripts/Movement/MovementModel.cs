using System;
using Installers;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Movement
{
    public class MovementModel : IInitializable, IDisposable, IMovementModel
    {
        protected float moveSpeed;
        private readonly float walkMoveSpeed;
        private readonly float sprintMoveSpeed;
        private readonly float crouchMoveSpeed;
   
        
        [Inject] private ISubject<MovementState> movementStates;
   
        private IDisposable movementStatesSubscription;
   
        public MovementModel(float walkMoveSpeed, float sprintMoveSpeed, float crouchMoveSpeed)
        {
            moveSpeed = walkMoveSpeed;
            this.walkMoveSpeed = walkMoveSpeed;
            this.sprintMoveSpeed = sprintMoveSpeed;
            this.crouchMoveSpeed = crouchMoveSpeed;
        }
   
        public virtual void Initialize()
        {
            movementStatesSubscription = movementStates.Subscribe(ChangeMoveSpeed);
        }
   
        public virtual void Dispose()
        {
            movementStatesSubscription.Dispose();
        }

        public virtual Vector2 GetMovement(Vector2 input)
        {
            return input * moveSpeed;
        }
        
        private void ChangeMoveSpeed(MovementState movementState)
        {
            moveSpeed = movementState switch
            {
                MovementState.Walking => walkMoveSpeed,
                MovementState.Sprinting => sprintMoveSpeed,
                MovementState.Crouched => crouchMoveSpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(movementState), movementState, "State not supported.")
            };
        }
    }
}
