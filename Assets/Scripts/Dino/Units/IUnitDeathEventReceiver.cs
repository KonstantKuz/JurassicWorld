namespace Dino.Units
{
    public interface IUnitDeathEventReceiver
    {
        void OnDeath(DeathCause deathCause);
    }
}