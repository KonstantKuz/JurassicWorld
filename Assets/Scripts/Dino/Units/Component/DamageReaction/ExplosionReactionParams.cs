using System;
using UnityEngine;

namespace Dino.Units.Component.DamageReaction
{
    [Serializable]
    public struct ExplosionReactionParams
    {
        public float JumpDistance;
        public float JumpHeight;
        public float JumpDuration;
        
        [HideInInspector]
        public Vector3 ExplosionPosition;
    }
}