using System;
using Installers;
using UniRx;
using UnityEngine.InputSystem;
using Zenject;

namespace Movement
{
    public class PlayerMovementStateController : IInitializable, IDisposable 
    {
        private enum StateChange
        {
            StopCrouching,
            StartCrouching,
            StopSprinting,
            StartSprinting
        }
        [Inject(Id = BindingIDs.CrouchInput)]
        private InputAction crouchInput;
        [Inject(Id = BindingIDs.SprintInput)]
        private InputAction sprintInput;

        private MovementState currentState;
        private MovementState CurrentState
        {
            get => currentState;
            set
            {
                movementStates.OnNext(value);
                currentState = value;
            }
        }

        [Inject] private ISubject<MovementState> movementStates;

        public void Initialize()
        {
            crouchInput.started += HandleCrouchStarted;
            crouchInput.canceled += HandleCrouchEnded;
            sprintInput.started += HandleSprintStarted;
            sprintInput.canceled += HandleSprintEnded;
        }

        public void Dispose()
        {
            crouchInput.started -= HandleCrouchStarted;
            crouchInput.canceled -= HandleCrouchEnded;
            sprintInput.started -= HandleSprintStarted;
            sprintInput.canceled -= HandleSprintEnded;
        }

        private void HandleCrouchStarted(InputAction.CallbackContext obj)
        {
            TransitionState(StateChange.StartCrouching);
        }

        private void HandleCrouchEnded(InputAction.CallbackContext obj)
        {
            TransitionState(StateChange.StopCrouching);
        }

        private void HandleSprintStarted(InputAction.CallbackContext obj)
        {
            TransitionState(StateChange.StartSprinting);
        }

        private void HandleSprintEnded(InputAction.CallbackContext obj)
        {
            TransitionState(StateChange.StopSprinting);
        }

        private void TransitionState(StateChange desiredState)
        {
            CurrentState = (CurrentState, desiredState) switch
            {
                (MovementState.Crouched, StateChange.StopCrouching) => MovementState.Walking,
                (MovementState.Crouched, StateChange.StartCrouching) => MovementState.Crouched,
                (MovementState.Crouched, StateChange.StopSprinting) => MovementState.Crouched,
                (MovementState.Crouched, StateChange.StartSprinting) => MovementState.Sprinting,
                (MovementState.Walking, StateChange.StopCrouching) => MovementState.Walking,
                (MovementState.Walking, StateChange.StartCrouching) => MovementState.Crouched,
                (MovementState.Walking, StateChange.StopSprinting) => MovementState.Walking,
                (MovementState.Walking, StateChange.StartSprinting) => MovementState.Sprinting,
                (MovementState.Sprinting, StateChange.StopCrouching) => MovementState.Sprinting,
                (MovementState.Sprinting, StateChange.StartCrouching) => MovementState.Sprinting,
                (MovementState.Sprinting, StateChange.StopSprinting) => MovementState.Walking,
                (MovementState.Sprinting, StateChange.StartSprinting) => MovementState.Sprinting,
                _ => throw new ArgumentOutOfRangeException($"State change {desiredState} from state {CurrentState} is not supported.")
            };
        }
    }
}
