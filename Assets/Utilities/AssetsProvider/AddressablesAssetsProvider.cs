using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utilities.AssetsProvider
{
    internal sealed class AddressablesAssetsProvider : IAssetsProvider, IDisposable
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public async UniTask<T> GetAssetAsync<T>(string path, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Addressable key/path is null or empty.", nameof(path));
            }

            string handleKey = BuildHandleKey<T>(path);
            if (_handles.TryGetValue(handleKey, out AsyncOperationHandle cachedHandle))
            {
                if (cachedHandle.IsValid() &&
                    cachedHandle.Status == AsyncOperationStatus.Succeeded &&
                    TryExtractAssetFromHandle(cachedHandle, out T cachedAsset))
                {
                    return cachedAsset;
                }

                if (cachedHandle.IsValid())
                {
                    Addressables.Release(cachedHandle);
                }

                _handles.Remove(handleKey);
            }

            AsyncOperationHandle handle = default;
            try
            {
                if (typeof(Component).IsAssignableFrom(typeof(T)))
                {
                    AsyncOperationHandle<GameObject> gameObjectHandle = Addressables.LoadAssetAsync<GameObject>(path);
                    handle = gameObjectHandle;
                    await gameObjectHandle.ToUniTask(cancellationToken: cancellationToken);
                }
                else
                {
                    AsyncOperationHandle<T> typedHandle = Addressables.LoadAssetAsync<T>(path);
                    handle = typedHandle;
                    await typedHandle.ToUniTask(cancellationToken: cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }

                throw;
            }

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception($"Failed to load addressable asset. Key: '{path}', Type: '{typeof(T).Name}'.");
            }

            if (!TryExtractAssetFromHandle(handle, out T loadedAsset))
            {
                throw new Exception(
                    $"Addressable key '{path}' loaded successfully, but asset can't be mapped to '{typeof(T).Name}'.");
            }

            _handles[handleKey] = handle;
            return loadedAsset;
        }

        public void ReleaseAsset<T>(string path)
            where T : UnityEngine.Object
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            string handleKey = BuildHandleKey<T>(path);
            if (!_handles.TryGetValue(handleKey, out AsyncOperationHandle handle))
            {
                return;
            }

            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }

            _handles.Remove(handleKey);
        }

        public void Dispose()
        {
            foreach (KeyValuePair<string, AsyncOperationHandle> pair in _handles)
            {
                if (pair.Value.IsValid())
                {
                    Addressables.Release(pair.Value);
                }
            }

            _handles.Clear();
        }

        private static string BuildHandleKey<T>(string path)
            where T : UnityEngine.Object
        {
            return $"{typeof(T).FullName}::{path}";
        }

        private static bool TryExtractAssetFromHandle<T>(AsyncOperationHandle handle, out T asset)
            where T : UnityEngine.Object
        {
            if (handle.Result is T directAsset)
            {
                asset = directAsset;
                return true;
            }

            if (typeof(Component).IsAssignableFrom(typeof(T)) && handle.Result is GameObject gameObject)
            {
                Component component = gameObject.GetComponent(typeof(T));
                if (component != null)
                {
                    asset = component as T;
                    return asset != null;
                }
            }

            asset = null;
            return false;
        }
    }
}
