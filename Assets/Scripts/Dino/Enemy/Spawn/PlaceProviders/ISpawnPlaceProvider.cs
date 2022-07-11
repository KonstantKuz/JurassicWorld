using Dino.Enemy.Spawn.Config;

namespace Dino.Enemy.Spawn.PlaceProviders
{
    public interface ISpawnPlaceProvider
    {
        SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry);
    }
}