using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IKGTools.VFXs.UI;
using UnityEngine;

namespace IKGTools.VFXs
{
    public sealed class VFXsService : MonoBehaviour, IVFXsService
    {
        [SerializeField]
        private VFXsServiceInstancesCreator _instancesCreator;
        
        private Dictionary<string, ParticlesPool> _pools;

        public async UniTask InitializeAsync(PreInitializeData preInitializeData = null)
        {
            if (preInitializeData != null)
            {
                var lazyIterator = preInitializeData.LazyIterator;
                
                var data = preInitializeData.GetPreInitializeInstancesCount();

                _pools = new Dictionary<string, ParticlesPool>(data.Count);

                foreach (var value in data)
                {
                    var pool = GetPoolForVFX(value.vfx);

                    for (int index = 0; index < value.preInitializeCount; index++)
                    {
                        AddVFXInstanceToPool(pool, value.vfx);
                        
                        await lazyIterator.WaitInstanceIterationAsync();
                    }
                }
            }
            else
            {
                _pools = new Dictionary<string, ParticlesPool>();
            }
        }
        
        async void IVFXsService.Raise(VFXData vfx, VFXParameters parameters)
        {
            var particle = GetParticleInstance(vfx, out ParticlesPool pool);
            var vfxInstance = (IVFX) particle;
            
            await vfxInstance.RaiseAsync(parameters);
            
            pool.Release(particle);
        }

        async UniTask IVFXsService.RaiseAsync(VFXData vfx, VFXParameters parameters)
        {
            var particle = GetParticleInstance(vfx, out ParticlesPool pool);
            var vfxInstance = (IVFX) particle;

            await vfxInstance.RaiseAsync(parameters);

            pool.Release(particle);
        }

        async void IUIVFXsService.Raise(VFXData vfx, VFXParameters parameters, Transform uiParent)
        {
            var particle = GetParticleInstance(vfx, out ParticlesPool pool, uiParent);
            var vfxInstance = (IVFX) particle;

            await vfxInstance.RaiseAsync(parameters);

            pool.Release(particle);
        }

        async UniTask IUIVFXsService.RaiseAsync(VFXData vfx, VFXParameters parameters, Transform uiParent)
        {
            var particle = GetParticleInstance(vfx, out ParticlesPool pool, uiParent);
            var vfxInstance = (IVFX) particle;
            
            await vfxInstance.RaiseAsync(parameters);
            
            pool.Release(particle);
        }


        private VFXComponent GetParticleInstance(VFXData vfx, out ParticlesPool pool, Transform customParent = null)
        {
            pool = GetPoolForVFX(vfx);

            if (pool.FreeObjectsCount == 0)
                AddVFXInstanceToPool(pool, vfx);

            if (customParent == null)
                return pool.Take();

            return pool.Take(customParent);
        }

        private void AddVFXInstanceToPool(ParticlesPool pool, VFXData vfx)
        {
            var instance = _instancesCreator.CreateInstance(vfx.Source, $"{vfx.Id}_{pool.Count+1:00}", Vector3.zero, null);
            pool.Add(instance);
        }

        private ParticlesPool GetPoolForVFX(VFXData vfx)
        {
            string key = vfx.Id;
            
            if (!_pools.ContainsKey(key))
            {
                CreateNewPoolFor(key);
            }

            return _pools[key];
        }

        private void CreateNewPoolFor(string key)
        {
            var poolInstance = _instancesCreator.CreateInstance<ParticlesPool>($"pool_{key}", Vector3.zero, transform);
            poolInstance.InitializePool();
            _pools.Add(key, poolInstance);
        }
    }
}