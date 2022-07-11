using Dino.Units.Model;
using UniRx;

namespace Dino.Units.Enemy.Model
{
    public class EnemyHealthModel : IHealthModel
    {
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth { get; }

        public EnemyHealthModel(int maxHealth)
        {
            StartingMaxHealth = maxHealth;
            MaxHealth = new ReactiveProperty<float>(maxHealth);
        }
    }
}