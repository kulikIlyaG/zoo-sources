using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zoo.Application.Configs
{
    internal sealed class Configs : IConfigs
    {
        private readonly IReadOnlyDictionary<Type, IConfig> _configs;

        public Configs(IReadOnlyDictionary<Type, IConfig> configs)
        {
            _configs = configs;
        }

        T IConfigs.GetConfig<T>()
        {
            Type type = typeof(T);

            if (_configs.TryGetValue(type, out IConfig config))
            {
                return (T) config;
            }

            Debug.LogError($"Not found config type: {type.Name}");

            return null;
        }
    }
}