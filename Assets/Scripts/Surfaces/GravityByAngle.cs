using System;
using UnityEngine;

namespace Surfaces
{
    [Serializable]
    public struct GravityByAngle
    {
        [Range(0,360)] public float angleStart;
        [Range(0,360)] public float angleEnd;
        [Min(0)] public float gravityForce;
    }
}