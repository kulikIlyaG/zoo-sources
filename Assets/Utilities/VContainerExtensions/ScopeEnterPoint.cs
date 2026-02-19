using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Utilities.VContainerExtensions
{
    public abstract class ScopeEnterPoint : MonoBehaviour, IAsyncStartable
    {
        public event Action OnFinished;
        
        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                await ExecuteEnterPointAsync(cancellationToken);
                
                OnFinished?.Invoke();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[{GetType().Name}.StartAsync(cts)] Operation canceled");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        protected abstract UniTask ExecuteEnterPointAsync(CancellationToken cancellationToken);
    }
}