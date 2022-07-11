﻿using UnityEngine;

namespace DinoWorldSurvival.Squad.Component
{
    public class SquadDestination : MonoBehaviour
    {
        public void SwitchVisibility()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.enabled = !meshRenderer.enabled;
        }
    }
}