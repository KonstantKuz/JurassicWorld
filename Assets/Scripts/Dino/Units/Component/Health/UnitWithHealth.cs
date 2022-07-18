using Feofun.Components;

namespace Dino.Units.Component.Health
{
    public class UnitWithHealth : Health, IInitializable<Unit>
    {
        public void Init(Unit unit)
        {
            base.Init(unit.Model.HealthModel);
        }
    }
}