
namespace Dino.Weapon.Model
{
    public interface IWeaponModel
    {
        float TargetSearchRadius { get; }
        float AttackDistance { get; }
        float AttackDamage { get; } 
        float AttackInterval { get; }
    }
}