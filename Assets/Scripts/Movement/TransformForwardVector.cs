using UnityEngine;
using Zenject;

namespace Movement
{
    public class TransformForwardVector 
    {
        private readonly Transform transform;

        public Vector3 forwardVector => transform.forward;

        public TransformForwardVector(Transform transform)
        {
            this.transform = transform;
        }
    }
}
