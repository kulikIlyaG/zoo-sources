using System.Collections.Generic;
using UnityEngine;

namespace Zoo.Gameplay.Entities
{
    internal sealed class EntitiesContactsResolver : IEntitiesContactsResolver
    {
        private const float REPEAT_CONTACT_THRESHOLD_MULTIPLIER = 1.5f;
        private readonly IReadOnlyList<ContactAction> _contactActions;

        private (ContactData data, float time) _lastContact;

        public EntitiesContactsResolver(IEnumerable<ContactAction> contactActions)
        {
            _contactActions = contactActions as IReadOnlyList<ContactAction> ?? new List<ContactAction>(contactActions);
        }
        
        void IEntitiesContactsResolver.OnContact(IEntity source, IEntity target, Collision collision)
        {
            var contactData = GetContactData(source, target, collision);
            if(contactData == null)
                return;

            if (ValidateRepeatedContact(contactData))
            {
                return;
            }
            
            ResolveContact(contactData);
        }

        private bool ValidateRepeatedContact(ContactData contactData)
        {
            float currentTime = Time.time;

            if (_lastContact.data == null)
            {
                _lastContact = (contactData, currentTime);
                return false;
            }

            bool isSameType = _lastContact.data.TypeId.Value == contactData.TypeId.Value;
            bool isSamePair =
                (_lastContact.data.A == contactData.A && _lastContact.data.B == contactData.B) ||
                (_lastContact.data.A == contactData.B && _lastContact.data.B == contactData.A);
            bool isInRepeatWindow = currentTime - _lastContact.time <= GetRepeatContactThreshold();

            bool isRepeated = isSameType && isSamePair && isInRepeatWindow;
            _lastContact = (contactData, currentTime);

            return isRepeated;
        }

        private float GetRepeatContactThreshold()
        {
            return Time.fixedDeltaTime * REPEAT_CONTACT_THRESHOLD_MULTIPLIER;
        }

        private static ContactData GetContactData(IEntity source, IEntity target, Collision collision)
        {
            return (source.Type, target.Type) switch
            {
                (Prey, Prey) => new ContactData(ContactTypeId.Of<PreyContactWithPrey>(), source, target, collision),
                (Prey, Predator) => new ContactData(ContactTypeId.Of<PredatorContactWithPrey>(), target, source, collision),
                (Predator, Prey) => new ContactData(ContactTypeId.Of<PredatorContactWithPrey>(), source, target, collision),
                (Predator, Predator) => new ContactData(ContactTypeId.Of<PredatorContactWithPredator>(), source, target, collision),
                _ => null
            };
        }

        private void ResolveContact(ContactData data)
        {
            foreach (ContactAction action in _contactActions)
            {
                if (action.IsSupport(data.TypeId))
                {
                    action.Execute(data);
                }
            }
        }
    }
}
