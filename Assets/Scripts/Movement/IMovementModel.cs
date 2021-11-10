using UnityEngine;

namespace Movement
{
    public interface IMovementModel
    {
        Vector2 GetMovement(Vector2 input);
    }
}