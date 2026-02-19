using System;
using IKGTools.ObjectsPool;
using Cysharp.Threading.Tasks;
using IKGTools.VFXs.SubElements;
using UnityEngine;
using UnityEngine.Events;

namespace IKGTools.VFXs
{
    public abstract class VFXComponent : MonoBehaviour, IVFX, IPoolableObject
    {
        [SerializeField] private SubVFXElement[] _subElements;
        
        [SerializeField] private UnityEvent _onRaise;
        
        void IVFX.Raise(VFXParameters parameters, Action onFinished)
        {
            if (parameters != null)
            {
                SetupParameters(parameters);
            }

            _onRaise?.Invoke();
            Raise(onFinished);
        }
        
        async UniTask IVFX.RaiseAsync(VFXParameters parameters)
        {
            if (parameters != null)
            {
                SetupParameters(parameters);
            }
            _onRaise?.Invoke();
            await RaiseAsync();
        }

        protected abstract void Raise(Action onFinished);
        protected abstract UniTask RaiseAsync();

        protected virtual void SetupParameters(VFXParameters parameters)
        {
            if (parameters.SubElements is {Length: > 0} && _subElements.Length > 0)
                SetSubParameters(parameters.SubElements);
            
            SetDefaultTransformParameters(parameters);
        }

        private void SetSubParameters(SubVFXElementParameter[] parameters)
        {
            foreach (var element in _subElements)
                element.SetParameters(parameters);
        }

        private void SetDefaultTransformParameters(VFXParameters parameters)
        {
            if (parameters.Rotation != null)
            {
                transform.rotation = parameters.Rotation.Value;
            }

            if (parameters.WorldPosition != null)
            {
                transform.position = parameters.WorldPosition.Value;
            }

            if (parameters.LocalScale != null)
            {
                transform.localScale = parameters.LocalScale.Value;
            }
        }

        public void OnAddedToPool()
        {
        }
        public void OnRemovedFromPool()
        {
        }
        public void OnGotIn()
        {
        }
        public void OnGotOut()
        {
        }
    }
}