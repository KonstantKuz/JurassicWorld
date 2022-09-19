using Feofun.ReceivingLoot.View;
using UnityEngine;

namespace Feofun.ReceivingLoot.Config
{
    [CreateAssetMenu(fileName = "ReceivedLootVfxConfig", menuName = "Feofun/ReceivedLootVfx")]
    public class ReceivedLootVfxConfig : ScriptableObject
    {
        public ReceivedLootView InstancePrefab;
        
        public float CreateDispersionX = 130;
        public float CreateDispersionY = 130;
        public float CreateDelay = 0.02f;
        
        public float ReceivedTime = 0.9f;
        public float ReceivedTimeDispersion = 0.2f;
        public float TimeBeforeReceive = 0.3f;
        
        public float MaxTrajectoryHeight = 400;
        public float MinTrajectoryHeight = 0;
        
        public float ScaleFactorBeforeReceive = 2; 
        public float FinalScaleFactor = 0.2f;
        public float RotationSpeedDispersion = 3f;
    }
}