using UnityEngine;
using VContainer;

namespace Utilities.UnityExtensions
{
    [RequireComponent(typeof(Collider))]
    internal sealed class BindableColliderComponent : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Component _component;
        
        private IBindableCollidersService _service;
        
        [Inject]
        private void Construct(IBindableCollidersService service)
        {
            _service = service;
        }

        private void Awake()
        {
            _service.RegisterCollider(_collider, _component);
        }

        private void OnDestroy()
        {
            _service.UnregisterCollider(_collider);
        }
    }
}