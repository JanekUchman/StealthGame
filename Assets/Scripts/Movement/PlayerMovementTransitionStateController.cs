using System;
using Cover;
using UniRx;
using Zenject;

namespace Movement
{
    public class PlayerMovementTransitionStateController : IInitializable, IDisposable
    {
        private enum StateChange
        {
            EnterCover,
            LeaveCover,
            EnterSlideToCover,
        }
        [Inject] private SignalBus signalBus;
        [Inject] private ISubject<MovementTransitionState> movementTransitionState;
        
        private MovementTransitionState currentState;
        private MovementTransitionState CurrentState
        {
            get => currentState;
            set
            {
                movementTransitionState.OnNext(value);
                currentState = value;
            }
        }
        
        public void Initialize()
        {
            //todo exit transition state
            signalBus.Subscribe<CoverFoundSignal>(TransitionToCover);
            signalBus.Subscribe<LeaveCoverSignal>(TransitionFromCover);
        }

        public void Dispose()
        {
            signalBus.Subscribe<CoverFoundSignal>(TransitionToCover);
            signalBus.Subscribe<LeaveCoverSignal>(TransitionFromCover);
        }

        private void TransitionToCover()
        {
            TransitionState(StateChange.EnterCover);
        }

        private void TransitionFromCover()
        {
            TransitionState(StateChange.LeaveCover);
        }
        
        private void TransitionState(StateChange desiredState)
        {
            CurrentState = (CurrentState, desiredState) switch
            {
                (MovementTransitionState.FreeMoving, StateChange.EnterCover) => MovementTransitionState.InCover,
                (MovementTransitionState.FreeMoving, StateChange.LeaveCover) => MovementTransitionState.FreeMoving,
                (MovementTransitionState.FreeMoving, StateChange.EnterSlideToCover) => MovementTransitionState.InTransition,
                (MovementTransitionState.InCover, StateChange.EnterCover) => MovementTransitionState.InCover,
                (MovementTransitionState.InCover, StateChange.LeaveCover) => MovementTransitionState.FreeMoving,
                (MovementTransitionState.InCover, StateChange.EnterSlideToCover) => MovementTransitionState.InTransition,
                (MovementTransitionState.InTransition, StateChange.EnterCover) => MovementTransitionState.InCover,
                (MovementTransitionState.InTransition, StateChange.LeaveCover) => MovementTransitionState.FreeMoving,
                (MovementTransitionState.InTransition, StateChange.EnterSlideToCover) => MovementTransitionState.InTransition,
                _ => throw new ArgumentOutOfRangeException($"State change {desiredState} from state {CurrentState} is not supported.")
            };
        }
    }
}
