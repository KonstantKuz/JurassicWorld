﻿using System.Runtime.Serialization;
using Dino.Weapon.Model;
using Feofun.Config;

namespace Dino.Weapon.Config
{
    [DataContract]
    public class WeaponConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")]
        private string _id;
        
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;
        
        [DataMember(Name = "AttackInterval")]
        private float _attackInterval;     
        
        [DataMember(Name = "Animation")]
        private string _animation;    
        
        [DataMember(Name = "AmmoId")]
        private string _ammoId;
        
        public float AttackDistance => _attackDistance;
        
        public int AttackDamage => _attackDamage;
        
        public float AttackInterval => _attackInterval;

        public string Animation => _animation;       
        public string AmmoId => _ammoId;

        public string Id => _id;
    }
}