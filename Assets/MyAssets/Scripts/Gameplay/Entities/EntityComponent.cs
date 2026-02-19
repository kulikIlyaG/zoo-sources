using IKGTools.ObjectsPool;
using UnityEngine;
using Utilities.UnityExtensions;
using VContainer;
using Zoo.Gameplay.Entities.Behaviours;
using Zoo.Gameplay.Movements;

namespace Zoo.Gameplay.Entities
{
    internal sealed class EntityComponent : MonoBehaviour, IEntity, IPoolableObject
    {
        [SerializeField] private CollisionEvents _collisionEvents;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        private string _sourceViewId;
        private EntityType _type;
        private IMovement _movement;
        private ISimpleBehaviour _behaviour;


        private IEntitiesController _entitiesController;
        private IObjectResolver _objectResolver;
        private IEntitiesContactsResolver _contactsResolver;

        private bool _isSleep;
        private float _sleepTime;

        public EntityType Type => _type;
        public string SourceViewId => _sourceViewId;

        IMovement IEntity.Movement => _movement;
        Vector3 IEntity.Position => transform.position;
        Rigidbody IEntity.Rigidbody => _rigidbody;

        [Inject]
        private void Construct(IEntitiesController entitiesController, IObjectResolver objectResolver,
            IEntitiesContactsResolver contactsResolver)
        {
            _entitiesController = entitiesController;
            _contactsResolver = contactsResolver;
            _objectResolver = objectResolver;
        }

        public void Setup(string sourceId, ISimpleBehaviour behaviour, IMovement movement, EntityType type, Vector3 startWorldPosition)
        {
            SetPosition(startWorldPosition);
            _sourceViewId = sourceId;
            _behaviour = behaviour;
            _movement = movement;
            _type = type;
        }


        public void Initialize()
        {
            _behaviour.Initialize(_objectResolver);
            _collisionEvents.OnEnter += OnCollisionWith;
        }


        void IEntity.Sleep(float seconds)
        {
            if (_isSleep)
                return;

            _sleepTime = seconds;
            _isSleep = true;
            _behaviour.OnSleepRequested();
        }

        void IEntity.StopSleep()
        {
            StopSleep_INTERNAL();
        }

        private void StopSleep_INTERNAL()
        {
            if (!_isSleep)
                return;

            _sleepTime = 0f;
            _isSleep = false;
            _behaviour.OnSleepFinished();
        }

        public void Tick()
        {
            if (_sleepTime > 0f)
            {
                _sleepTime -= Time.deltaTime;
                return;
            }

            if (_isSleep)
            {
                StopSleep_INTERNAL();
            }

            _behaviour.Tick();
        }

        private void OnCollisionWith(Collision collision)
        {
            if (_entitiesController.TryGetEntityByCollider(collision.collider, out EntityComponent collidedEntity))
            {
                OnCollideWithEntity(collidedEntity, collision);
            }
        }

        private void OnCollideWithEntity(EntityComponent collidedEntity, Collision collisionData)
        {
            _contactsResolver.OnContact(this, collidedEntity, collisionData);
        }

        public void DeInitialize()
        {
            _behaviour.DeInitialize();
            _collisionEvents.OnEnter -= OnCollisionWith;
        }

        private void OnDestroy()
        {
            DeInitialize();
        }
        
        private void ResetStates()
        {
            _isSleep = false;
            _sleepTime = 0f;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.linearVelocity = Vector3.zero;
        }
        
        private void SetPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
            _rigidbody.position = worldPosition;
        }

#region POOL EVENTS

        public void OnAddedToPool()
        {

        }

        public void OnRemovedFromPool()
        {
        }

        public void OnGotIn()
        {
            ResetStates();
            SetPosition(Vector3.one * -100f);
        }

        public void OnGotOut()
        {
        }

#endregion
    }

    public interface IEntity
    {
        EntityType Type { get; }
        IMovement Movement { get; }
        Vector3 Position { get; }
        Rigidbody Rigidbody { get; }
        
        void Setup(string viewSourceId, ISimpleBehaviour behaviour, IMovement movement, EntityType entityType, Vector3 startWorldPosition);
        void Sleep(float seconds);
        void StopSleep();
    }
}
