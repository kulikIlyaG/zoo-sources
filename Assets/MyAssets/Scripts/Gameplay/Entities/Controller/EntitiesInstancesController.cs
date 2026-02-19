using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.AssetsProvider;
using VContainer;
using VContainer.Unity;
using Zoo.Application.Configs;
using Zoo.Application.Keys;

namespace Zoo.Gameplay.Entities
{
    internal sealed class EntitiesInstancesController : MonoBehaviour
    {
        private Dictionary<string, EntitiesPool> _pools;
        
        private IAssetsProvider _assetsProvider;
        private IObjectResolver _objectResolver;
        
        [Inject]
        public void Construct(IAssetsProvider assetsProvider, IObjectResolver objectResolver, IConfigs configs)
        {
            _assetsProvider = assetsProvider;
            _objectResolver = objectResolver;
            
            _pools = new Dictionary<string, EntitiesPool>(configs.GetConfig<IEntitiesConfig>().Collection.Count);
        }
        
        public async UniTask<EntityComponent> GetInstanceAsync(string viewId)
        {
            var pool = GetPoolByEntityTypeId(viewId);

            if (pool.FreeObjectsCount == 0)
                await AddEntityInstanceToPoolAsync(pool, viewId);

            return pool.Take();
        }

        public void ReleaseInstances(EntityComponent entityComponent)
        {
            var pool = GetPoolByEntityTypeId(entityComponent.SourceViewId);
            
            pool.Release(entityComponent);
        }
        
        private async UniTask AddEntityInstanceToPoolAsync(EntitiesPool pool, string viewId)
        {
            var prefab = await _assetsProvider.GetAssetAsync<EntityComponent>(AssetsPaths.GetPathForEntity(viewId));
            var instance = CreateInstance(prefab, $"{prefab.name}_{pool.Count+1:00}", Vector3.zero, null);
            pool.Add(instance);
        }
        
        private EntitiesPool GetPoolByEntityTypeId(string viewId)
        {
            if (!_pools.ContainsKey(viewId))
            {
                CreateNewPoolFor(viewId);
            }

            return _pools[viewId];
        }

        private void CreateNewPoolFor(string viewId)
        {
            EntitiesPool poolInstance = CreateInstance<EntitiesPool>($"pool_{viewId}", Vector3.zero, transform);
            poolInstance.InitializePool();
            _pools.Add(viewId, poolInstance);
        }
        
        private T CreateInstance<T>(string name, Vector3 position, Transform parent) where T : MonoBehaviour
        {
            var instance = new GameObject(name);
            instance.transform.SetParent(parent);
            instance.transform.position = position;
            var component = instance.AddComponent<T>();
            
            _objectResolver.InjectGameObject(instance);
            
            return component;
        }
        
        private T CreateInstance<T>(T source, string name, Vector3 position, Transform parent) where T : MonoBehaviour
        {
            var instance = _objectResolver.Instantiate(source, position, Quaternion.identity, parent);
            instance.name = name;
            
            return instance;
        }
    }
}