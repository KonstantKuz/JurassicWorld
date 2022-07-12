
namespace Dino.Units.Model
{
    public interface IWeapon
    {
        float TargetSearchRadius { get; }
        float AttackDistance { get; }
        float AttackDamage { get; } 
        float AttackInterval { get; }
    }
}