using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cover
{
    public class Cover : MonoBehaviour, ICover
    {
        public Vector3 Position => transform.position;
    }
}

