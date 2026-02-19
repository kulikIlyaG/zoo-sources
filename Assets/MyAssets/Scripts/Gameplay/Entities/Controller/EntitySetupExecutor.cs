using UnityEngine;
using Zoo.Application.Configs;
using Zoo.Gameplay.Entities.Behaviours;
using Zoo.Gameplay.Movements;

namespace Zoo.Gameplay.Entities
{
    internal static class EntitySetupExecutor
    {
        public static void SetupEntityInstance(IEntity instance, EntityDescription description, Vector3 worldPosition)
        {
            IMovement movement = description.MovementType.CreateMovement(instance.Rigidbody);
            ISimpleBehaviour behaviour = description.BehaviourType.CreateBehaviour(instance);
            EntityType entityType = description.Type;
            instance.Setup(description.Id, behaviour, movement, entityType, worldPosition);
        }
    }
}