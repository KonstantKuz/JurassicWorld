using DinoWorldSurvival.Enemy.Spawn.Config;

namespace DinoWorldSurvival.Enemy.Spawn.PlaceProviders
{
    public interface ISpawnPlaceProvider
    {
        SpawnPlace GetSpawnPlace(EnemyWaveConfig waveConfig, int rangeTry);
    }
}