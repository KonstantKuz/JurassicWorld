using Feofun.Components;

namespace DinoWorldSurvival.Units.Component.Health
{
    public class UnitWithHealth : Health, IInitializable<IUnit>
    {
        public void Init(IUnit unit)
        {
            base.Init(unit.Model.HealthModel);
        }
    }
}