using UnityEngine;

namespace Dino.Enemy.Spawn
{
    public struct SpawnPlace
    {
        public static SpawnPlace INVALID = new SpawnPlace {IsValid = false};

        public bool IsValid;
        public Vector3 Position;
    }
}