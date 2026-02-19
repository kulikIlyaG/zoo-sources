using UnityEngine;

namespace Zoo.Gameplay.Entities
{
    internal sealed class ContactData
    {
        public readonly ContactTypeId TypeId;
        public readonly IEntity A;
        public readonly IEntity B;
        public readonly Collision Collision;

        public ContactData(ContactTypeId typeId, IEntity a, IEntity b, Collision collision)
        {
            TypeId = typeId;
            A = a;
            B = b;
            Collision = collision;
        }
    }
}