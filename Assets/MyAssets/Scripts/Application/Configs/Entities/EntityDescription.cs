using System;
using UnityEngine;
using Utilities.UnityExtensions;
using Zoo.Gameplay.Entities;
using Zoo.Gameplay.Entities.Behaviours;
using Zoo.Gameplay.Movements;

namespace Zoo.Application.Configs
{
    [Serializable]
    public sealed class EntityDescription
    {
        [SerializeField] private string _id;
        [SerializeReference, SReferenceView] private MovementType _movementType;
        [SerializeReference, SReferenceView] private SimpleBehaviourType _behaviourType;
        [SerializeReference, SReferenceView] private EntityType _type;

        public string Id => _id;
        public MovementType MovementType => _movementType;
        public SimpleBehaviourType BehaviourType => _behaviourType;
        public EntityType Type => _type;
    }
}
