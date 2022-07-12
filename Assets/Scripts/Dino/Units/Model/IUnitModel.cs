
namespace Dino.Units.Model
{
    public interface IUnitModel
    {
        string Id { get; }
        float MoveSpeed { get; }
        IHealthModel HealthModel { get; }
        IAttackModel AttackModel { get; }
    }
}