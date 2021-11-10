using System;
using System.Collections.Generic;
using Installers;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Movement
{
    public class PlayerMovementHandler : IInitializable, IDisposable, IPlayerMovementHandler
    {
        public event Action<Vector2> MoveCharacter;

        [Inject] private Dictionary<MovementTransitionState, IMovementModel> movementModels;
        [Inject] private ISubject<MovementTransitionState> movementTransitionStates;
        private IMovementModel currentMovementModel;
        [Inject(Id = BindingIDs.MovementInput)]
        private InputAction movementInput;

        public void Initialize()
        {
            currentMovementModel = movementModels[MovementTransitionState.FreeMoving];
            movementInput.started += GetMovement;
            movementTransitionStates.Subscribe(UpdateMovementInput);
        }

        public void Dispose()
        {
            movementInput.started -= GetMovement;
        }

        private void UpdateMovementInput(MovementTransitionState movementTransitionState)
        {
            currentMovementModel = movementModels[movementTransitionState];
        }

        private void GetMovement(InputAction.CallbackContext directionalInput)
        {
            MoveCharacter?.Invoke(currentMovementModel.GetMovement(directionalInput.ReadValue<Vector2>()));
        }
    }
}
