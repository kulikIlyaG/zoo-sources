using System;
using UnityEngine;

namespace Utilities.UnityExtensions
{
    public sealed class CollisionEvents : MonoBehaviour
    {
        public event Action<Collision> OnEnter, OnExit;
        
        private void OnCollisionEnter(Collision other)
        {
            OnEnter?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnExit?.Invoke(other);
        }
    }
}