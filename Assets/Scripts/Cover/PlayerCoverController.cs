using System;
using System.Collections.Generic;
using System.Linq;
using Installers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Cover
{
    public class PlayerCoverController : IInitializable, IDisposable
    {
        [Inject(Id = BindingIDs.SprintInput)]
        private InputAction coverInput;

        [Inject(Id = BindingIDs.PlayerTransform)]
        private Transform playerTransform;

        [Inject] private List<ICover> coverModels;

        [Inject] private SignalBus signalBus;
        private bool isInCover;

        private const float MaximumDistanceToCover = 0.5f;

        public void Initialize()
        {
            coverInput.started += SearchForCover;
        }

        public void Dispose()
        {
            coverInput.started -= SearchForCover;
        }

        private void SearchForCover(InputAction.CallbackContext obj)
        {
            if (!isInCover)
            {
                var closest = coverModels.OrderByDescending(x => Vector3.Distance(playerTransform.position, x.Position))
                    .FirstOrDefault();
                if (Vector3.Distance(closest.Position, playerTransform.position) <= MaximumDistanceToCover)
                {
                    isInCover = true;
                    signalBus.Fire(new CoverFoundSignal(closest));
                }
            }
            else
            {
                isInCover = true;
                signalBus.Fire<LeaveCoverSignal>();
            }
        }
    }
}
