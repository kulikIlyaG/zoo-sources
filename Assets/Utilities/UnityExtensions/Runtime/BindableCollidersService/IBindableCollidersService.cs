using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.UnityExtensions
{
    public interface IBindableCollidersService
    {
        void RegisterCollider(Collider collider, Component component);
        void UnregisterCollider(Collider collider);
        
        bool TryGetRegisteredComponent<T>(Collider collider, out T component) where T : Component;
        bool TryGetRegisteredComponent(Collider collider, out Component component);
    }

    internal sealed class BindableCollidersService : IBindableCollidersService, IDisposable
    {
        private readonly Dictionary<Collider, Component> _registered = new();
        
        void IBindableCollidersService.RegisterCollider(Collider collider, Component component)
        {
            _registered.Add(collider, component);
        }

        void IBindableCollidersService.UnregisterCollider(Collider collider)
        {
            _registered.Remove(collider);
        }

        bool IBindableCollidersService.TryGetRegisteredComponent<T>(Collider collider, out T component)
        {
            var isRegistered = TryGetRegisteredComponent_INTERNAL(collider, out Component originalComponent);
            if (isRegistered)
            {
                if (originalComponent is T casted)
                {
                    component = casted;
                    return true;
                }
            }

            component = null;
            return false;
        }
        
        bool IBindableCollidersService.TryGetRegisteredComponent(Collider collider, out Component component)
        {
            return TryGetRegisteredComponent_INTERNAL(collider, out component);
        }

        private bool TryGetRegisteredComponent_INTERNAL(Collider collider, out Component component)
        {
            return _registered.TryGetValue(collider, out component);
        }

        public void Dispose()
        {
            _registered.Clear();
        }
    }
}