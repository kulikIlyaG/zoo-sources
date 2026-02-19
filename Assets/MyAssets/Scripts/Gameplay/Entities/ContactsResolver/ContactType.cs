using System;

namespace Zoo.Gameplay.Entities
{
    internal abstract class ContactType {}

    internal readonly struct ContactTypeId
    {
        public Type Value { get; }

        private ContactTypeId(Type value)
        {
            Value = value;
        }

        public static ContactTypeId Of<TContactType>()
            where TContactType : ContactType
        {
            return new ContactTypeId(typeof(TContactType));
        }
    }

}
