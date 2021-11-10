using System;
using Cover;
using Installers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Movement
{
    public class SlidingMovementModel : MovementModel
    {
        [Inject] private SignalBus signalBus;
        [Inject(Id = BindingIDs.PlayerTransform)]
        private Transform playerTransform;
        private ICover targetCover;
        private readonly float slideMoveSpeed;
        
        public SlidingMovementModel(float walkMoveSpeed, float sprintMoveSpeed, float crouchMoveSpeed, float slideMoveSpeed) : base(walkMoveSpeed, sprintMoveSpeed, crouchMoveSpeed)
        {
            this.slideMoveSpeed = slideMoveSpeed;
        }

        public override void Initialize()
        {
            base.Initialize();
            signalBus.Subscribe<CoverFoundSignal>(RegisterCover);
        }

        public override void Dispose()
        {
            base.Dispose();
            signalBus.Unsubscribe<CoverFoundSignal>(RegisterCover);
        }

        public override Vector2 GetMovement(Vector2 input)
        {
            return (playerTransform.position - targetCover.Position) * slideMoveSpeed;
        }

        private void RegisterCover(CoverFoundSignal coverFoundSignal)
        {
            targetCover = coverFoundSignal.cover;
        }
    }
}
