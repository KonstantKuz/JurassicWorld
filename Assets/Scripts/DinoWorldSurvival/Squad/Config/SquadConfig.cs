﻿using System.Runtime.Serialization;

namespace Survivors.Squad.Config
{
    public class SquadConfig
    {
        [DataMember] 
        public float Speed;
        [DataMember]
        public float CollectRadius;
    }
}