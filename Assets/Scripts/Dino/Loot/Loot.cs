using Dino.Location.Model;
using UnityEngine;

namespace Dino.Loot
{
    public class Loot : WorldObject
    {
        public float CollectProgress { get; private set; } = 0f;

        public void IncreaseCollectProgress()
        {
            CollectProgress += Time.deltaTime;
        }
    }
}