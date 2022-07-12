using System.Runtime.Serialization;
using Dino.Weapon.Model;
using Feofun.Config;

namespace Dino.Weapon.Config
{
    [DataContract]
    public class WeaponConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")]
        private WeaponId _id;
        
        [DataMember(Name = "AttackDistance")]
        private float _attackDistance;

        [DataMember(Name = "AttackDamage")]
        private int _attackDamage;
        
        [DataMember(Name = "AttackInterval")]
        private float _attackInterval;
        
        
        public float AttackDistance => _attackDistance;
        
        public int AttackDamage => _attackDamage;
        
        public float AttackInterval => _attackInterval;

        public string Id => _id.ToString();
    }
}