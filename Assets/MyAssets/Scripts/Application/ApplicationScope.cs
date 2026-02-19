using System;
using System.Collections.Generic;
using IKGTools.Services.Timers;
using UnityEngine;
using Utilities.AssetsProvider;
using Utilities.VContainerExtensions;
using VContainer;
using Zoo.Application.Configs;

namespace Zoo.Application
{
    internal sealed class ApplicationScope : BaseScope
    {
        [Header("Configs")]
        [SerializeField] private EntitiesConfigSO _entitiesConfig;
        [SerializeField] private SpawnEntitiesConfigSO _spawnEntitiesConfig;
        [SerializeField] private GameplayVFXsConfigSO _gameplayVFXsConfig;
        
        protected override void ConfigureProcess(IContainerBuilder builder)
        {
            ConfigureConfigs(builder);
            
            TimerServiceInstaller.Install(builder);
            AssetsProviderInstaller.Install(builder);
        }

        private void ConfigureConfigs(IContainerBuilder builder)
        {
            IReadOnlyDictionary<Type, IConfig> configs = new Dictionary<Type, IConfig>
            {
                {typeof(IEntitiesConfig), _entitiesConfig},
                {typeof(ISpawnEntitiesConfig), _spawnEntitiesConfig},
                {typeof(IGameplayVFXsConfig), _gameplayVFXsConfig}
            };
            
            builder.Register<Configs.Configs>(Lifetime.Singleton).As<IConfigs>().WithParameter(configs);
        }
    }
}
