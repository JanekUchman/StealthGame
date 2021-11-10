using System;
using UnityEngine;

namespace Movement
{
    public interface IPlayerMovementHandler
    {
        event Action<Vector2> MoveCharacter;
    }
}