using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using IKGTools.Services.Timers;
using Zoo.Application.Configs;
using Random = UnityEngine.Random;

namespace Zoo.Gameplay.Entities
{
    public interface IEntitiesSpawnSystem
    {
        void StartSpawning();
    }
    
    internal sealed class EntitiesSpawnSystem : IDisposable, IEntitiesSpawnSystem
    {
        private readonly IEntitiesController _controller;
        private readonly IEntitiesConfig _entitiesConfig;
        private readonly ISpawnEntitiesConfig _spawnEntitiesConfig;
        private readonly ITimersService _timerService;
        
        private readonly ISpawnPositionProvider _spawnPositionProvider;

        private readonly IReadOnlyList<string> _entitiesViewIds;
        
        private ushort? _timerId;
        
        public EntitiesSpawnSystem(IEntitiesController controller, IConfigs configs, ITimersService timerService)
        {
            _controller = controller;

            _spawnPositionProvider = new SpawnPositionProviderZero();
            
            _entitiesConfig = configs.GetConfig<IEntitiesConfig>();
            _spawnEntitiesConfig = configs.GetConfig<ISpawnEntitiesConfig>();
            _timerService = timerService;

            _entitiesViewIds = _entitiesConfig.Collection.Keys.ToList();
        }

        public void StartSpawning()
        {
            StartSpawnIteration();
        }
        
        private async UniTask SpawnEntityAsync()
        {
            await _controller.EntityStartLifeAsync(GetEntityDescription(), _spawnPositionProvider.GetSpawnPosition());
        }

        private EntityDescription GetEntityDescription()
        {
            return _entitiesConfig.Collection[_entitiesViewIds[Random.Range(0, _entitiesViewIds.Count)]];
        }

        
        public void Dispose()
        {
            if (_timerId.HasValue)
            {
                if (_timerService.IsTimerRunning(_timerId.Value))
                {
                    _timerService.KillTimer(_timerId.Value);
                }
            }
        }

        private void StartSpawnIteration()
        {
            _timerId = _timerService.CreateTimer(GetSpawnRate(), OnTimerFinished);
        }

        private async void OnTimerFinished()
        {
            await SpawnEntityAsync();
            StartSpawnIteration();
        }

        private float GetSpawnRate()
        {
            return Random.Range(_spawnEntitiesConfig.SpawnEntitiesRateMin, _spawnEntitiesConfig.SpawnEntitiesRateMax);
        }
    }
}