using System.Runtime.Serialization;
using Dino.Weapon.Model;
using Feofun.Config;

namespace Dino.Weapon.Config
{
    [DataContract]
    public class WeaponConfig : ICollectionItem<WeaponId>
    {
        [DataMember(Name = "Id")]
        private WeaponId _id;
        
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;
        
        [DataMember(Name = "AttackInterval")]
        private float _attackInterval;     
        
        [DataMember(Name = "Animation")]
        private string _animation;    
        
        [DataMember(Name = "Ammo")]
        private string _ammo;
        
        public float AttackDistance => _attackDistance;
        
        public int AttackDamage => _attackDamage;
        
        public float AttackInterval => _attackInterval;

        public string Animation => _animation;       
        public string Ammo => _ammo;

        public WeaponId Id => _id;
    }
}