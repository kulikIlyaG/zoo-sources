using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utilities.UnityExtensions;
using VContainer;
using VContainer.Unity;
using Zoo.Application.Configs;

namespace Zoo.Gameplay.Entities
{
    public interface IEntitiesController
    {
        UniTask EntityStartLifeAsync(EntityDescription entityDescription, Vector3 worldPosition);
        void EntityEndLife(IEntity entity);
        
        
        internal bool TryGetEntityByCollider(Collider possibleEntityCollider, out EntityComponent entityComponent);
    }
    
    internal sealed class EntitiesController : MonoBehaviour, ITickable, IEntitiesController
    {
        [SerializeField] private EntitiesInstancesController _instancesController;
        
        private readonly Dictionary<IEntity, EntityComponent> _activeEntities = new();
        
        private readonly HashSet<EntityComponent> _lazyActivateInstances = new();
        private readonly HashSet<EntityComponent> _lazyDeactivateInstances = new();
        private bool _isTickInProgress;

        private IBindableCollidersService _bindableCollidersService;

        [Inject]
        public void Construct(IBindableCollidersService bindableCollidersService)
        {
            _bindableCollidersService = bindableCollidersService;
        }
        
        
        async UniTask IEntitiesController.EntityStartLifeAsync(EntityDescription entityDescription, Vector3 worldPosition)
        {
            var instance = await _instancesController.GetInstanceAsync(entityDescription.Id);
            
            EntitySetupExecutor.SetupEntityInstance(instance, entityDescription, worldPosition);
            
            instance.Initialize();
            
            LazyActivateEntity(instance);
        }


        bool IEntitiesController.TryGetEntityByCollider(Collider possibleEntityCollider, out EntityComponent entityComponent)
        {
            return _bindableCollidersService.TryGetRegisteredComponent(possibleEntityCollider, out entityComponent);
        }

        void IEntitiesController.EntityEndLife(IEntity entity)
        {
            if (!TryGetActiveEntityComponent(entity, out var entityComponent))
            {
                return;
            }
            
            entityComponent.DeInitialize();
            
            LazyDeactivateEntity(entity, entityComponent);
        }
        
        
        public void Tick()
        {
            ApplyLazyActivityChanges();

            _isTickInProgress = true;
            try
            {
                foreach (var entityPair in _activeEntities)
                {
                    entityPair.Value.Tick();
                }
            }
            finally
            {
                _isTickInProgress = false;
                ApplyLazyActivityChanges();
            }
        }
        
        
        private void ReleaseEntityInstance(EntityComponent entity)
        {
            _instancesController.ReleaseInstances(entity);
        }

        private bool TryGetActiveEntityComponent(IEntity entity, out EntityComponent entityComponent)
        {
            return _activeEntities.TryGetValue(entity, out entityComponent);
        }
        
        private void LazyActivateEntity(EntityComponent instance)
        {
            if (_isTickInProgress)
            {
                _lazyDeactivateInstances.Remove(instance);
                _lazyActivateInstances.Add(instance);
                return;
            }

            _activeEntities.Add(instance, instance);
        }
        
        private void LazyDeactivateEntity(IEntity entity, EntityComponent entityComponent)
        {
            if (_isTickInProgress)
            {
                _lazyActivateInstances.Remove(entityComponent);
                _lazyDeactivateInstances.Add(entityComponent);
                return;
            }

            ReleaseEntityInstance(entityComponent);
            _activeEntities.Remove(entity);
        }
        
        private void ApplyLazyActivityChanges()
        {
            if (_lazyDeactivateInstances.Count > 0)
            {
                foreach (EntityComponent entity in _lazyDeactivateInstances)
                {
                    _activeEntities.Remove(entity);
                    ReleaseEntityInstance(entity);
                }
                _lazyDeactivateInstances.Clear();
            }

            if (_lazyActivateInstances.Count > 0)
            {
                foreach (EntityComponent entity in _lazyActivateInstances)
                {
                    _activeEntities.Add(entity, entity);
                }
                _lazyActivateInstances.Clear();
            }
        }
    }
}
