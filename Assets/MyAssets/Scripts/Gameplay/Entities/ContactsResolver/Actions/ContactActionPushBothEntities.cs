using UnityEngine;

namespace Zoo.Gameplay.Entities
{
    internal sealed class ContactActionPushBothEntities : ContactAction
    {
        private const float ADD_FORCE_POWER = 100;
        private const float SLEEP_AFTER_POWER = 1f;

        public ContactActionPushBothEntities()
            : base(
                
                ContactTypeId.Of<PreyContactWithPrey>()
                
                ){}

        public override void Execute(ContactData contactData)
        {
            Vector3 collisionNormal = GetCollisionNormal(contactData);

            float forcePower = GetForcePower(contactData.Collision);
            float sleepTime = GetSleepTime(contactData.Collision);
            
            contactData.A.Sleep(sleepTime);
            contactData.B.Sleep(sleepTime);
            contactData.A.Rigidbody.AddForce(collisionNormal * forcePower);
            contactData.B.Rigidbody.AddForce(-collisionNormal * forcePower);
        }

        private float GetSleepTime(Collision collisionData)
        {
            return SLEEP_AFTER_POWER;
        }

        private float GetForcePower(Collision collisionData)
        {
            return ADD_FORCE_POWER;
        }

        private Vector3 GetCollisionNormal(ContactData contactData)
        {
            Vector3 normal;

            if (contactData.Collision != null && contactData.Collision.contactCount > 0)
            {
                normal = contactData.Collision.GetContact(0).normal;
            }
            else
            {
                normal = (contactData.A.Position - contactData.B.Position).normalized;
            }

            if (normal.y < 0f)
            {
                normal.y = 0f;
            }

            return normal.normalized;
        }
    }
}