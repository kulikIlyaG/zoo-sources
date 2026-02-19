using System;
using System.Collections.Generic;

namespace Zoo.Gameplay.Entities
{
    
    internal abstract class ContactAction
    {
        private readonly HashSet<Type> _supportedTypes;

        protected ContactAction(params ContactTypeId[] supportedTypes)
        {
            var types = new HashSet<Type>();
            if (supportedTypes != null)
            {
                foreach (ContactTypeId supportedType in supportedTypes)
                {
                    types.Add(supportedType.Value);
                }
            }

            _supportedTypes = types;
        }

        public bool IsSupport(ContactTypeId contactType)
        {
            return _supportedTypes.Contains(contactType.Value);
        }
        
        public abstract void Execute(ContactData contactData);
    }
}