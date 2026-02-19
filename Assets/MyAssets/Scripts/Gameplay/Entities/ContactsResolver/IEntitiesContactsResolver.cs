using UnityEngine;

namespace Zoo.Gameplay.Entities
{
    public interface IEntitiesContactsResolver
    {
        void OnContact(IEntity source, IEntity target, Collision collision);
    }
}