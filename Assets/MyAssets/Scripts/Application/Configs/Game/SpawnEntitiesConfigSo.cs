using UnityEngine;

namespace Zoo.Application.Configs.Game
{
    
    [CreateAssetMenu(fileName = "SpawnEntitiesConfig", menuName = "Configs/Spawn Entities")]
    internal sealed class SpawnEntitiesConfigSo : ScriptableObject, ISpawnEntitiesConfig
    {
        [SerializeField] private float _spawnEntitiesRateMin = 1f;
        [SerializeField] private float _spawnEntitiesRateMax = 2f;

        
        public float SpawnEntitiesRateMin => _spawnEntitiesRateMin;
        public float SpawnEntitiesRateMax => _spawnEntitiesRateMax;
    }
}