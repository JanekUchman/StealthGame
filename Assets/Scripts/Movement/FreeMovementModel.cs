using System;
using Installers;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Movement
{
    public class FreeMovementModel : IInitializable, IDisposable
    {
        private float moveSpeed;
        private float walkMoveSpeed;
        private float sprintMoveSpeed;
        private float crouchMoveSpeed;

        [Inject(Id = BindingIDs.MovementInput)]
        private InputAction movementInput;
        [Inject] private ISubject<MovementState> movementStates;
        [Inject] private ISubject<MovementTransitionStates> movementTransitionStates;

        public event Action<Vector2> MoveCharacter;
        private IDisposable movementStatesSubscription;
        private IDisposable movementTransitionStatesSubscription;

        public FreeMovementModel(float walkMoveSpeed, float sprintMoveSpeed, float crouchMoveSpeed)
        {
            moveSpeed = walkMoveSpeed;
            this.walkMoveSpeed = walkMoveSpeed;
            this.sprintMoveSpeed = sprintMoveSpeed;
            this.crouchMoveSpeed = crouchMoveSpeed;
        }

        public void Initialize()
        {
            movementInput.started += ProcessMovementInput;
            movementInput.canceled += StopMovementInput;
            movementStatesSubscription = movementStates.Subscribe(ChangeMoveSpeed);
            movementTransitionStatesSubscription = movementTransitionStates.Subscribe(ProcessStateChange);
        }

        public void Dispose()
        {
            movementInput.started -= ProcessMovementInput;
            movementInput.canceled -= StopMovementInput;
        }

        private void StopMovementInput(InputAction.CallbackContext obj)
        {
            var movement = obj.ReadValue<Vector2>();
        }

        private void ProcessMovementInput(InputAction.CallbackContext obj)
        {
            
        }

        private void ProcessStateChange(MovementTransitionStates movementTransitionStates)
        {
            
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
