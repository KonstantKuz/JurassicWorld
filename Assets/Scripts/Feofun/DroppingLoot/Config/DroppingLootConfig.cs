using Feofun.DroppingLoot.View;
using UnityEngine;

namespace Feofun.DroppingLoot.Config
{
    [CreateAssetMenu(fileName = "DroppingLootConfig", menuName = "Feofun/DroppingLoot")]
    public class DroppingLootConfig : ScriptableObject
    {
        public DroppingObjectView InstancePrefab;
        
        public float CreateDispersionX = 130;
        public float CreateDispersionY = 130;
        public float CreateDelay = 0.02f;
        
        public float DroppingTime = 0.9f;
        public float DroppingTimeDispersion = 0.2f;
        public float TimeBeforeDrop = 0.3f;
        
        public float MaxTrajectoryHeight = 400;
        public float MinTrajectoryHeight = 0;
        
        public float ScaleFactorBeforeDrop = 2; 
        public float FinalScaleFactor = 0.2f;
        public float RotationSpeedDispersion = 3f;
    }
}