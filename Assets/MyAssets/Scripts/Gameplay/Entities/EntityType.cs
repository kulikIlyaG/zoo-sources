using System;

namespace Zoo.Gameplay.Entities
{
    [Serializable]
    public abstract class EntityType {}
    
    [Serializable]
    public sealed class Prey : EntityType {}
    
    [Serializable]
    public sealed class Predator : EntityType {}
}