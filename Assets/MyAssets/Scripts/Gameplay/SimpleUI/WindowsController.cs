using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Zoo.Gameplay.SimpleUI
{
    internal sealed class WindowsController : MonoBehaviour, IUIService
    {
        private readonly Dictionary<Type, WindowPresenter> _windows = new();

        [Inject]
        public void Construct(IEnumerable<WindowPresenter> presenters)
        {
            foreach (var p in presenters)
                _windows[p.GetType()] = p;
        }

        UniTask IUIService.ShowAsync<TPresenter>(bool immediately, CancellationToken ct)
        {
            return Get<TPresenter>().ShowAsync(immediately, ct);  
        }

        UniTask IUIService.HideAsync<TPresenter>(bool immediately, CancellationToken ct)
        {
            return Get<TPresenter>().HideAsync(immediately, ct);
        }

        private TPresenter Get<TPresenter>() where TPresenter : WindowPresenter
        {
            if (_windows.TryGetValue(typeof(TPresenter), out var presenter))
                return (TPresenter)presenter;

            throw new Exception($"Window presenter not registered: {typeof(TPresenter).Name}");
        }
    }
}