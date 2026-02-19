namespace Zoo.Application.Configs
{
    public interface ISpawnEntitiesConfig : IConfig
    {
        float SpawnEntitiesRateMin { get; }
        float SpawnEntitiesRateMax { get; }
    }
}