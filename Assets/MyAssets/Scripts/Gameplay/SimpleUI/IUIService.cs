using System.Threading;
using Cysharp.Threading.Tasks;

namespace Zoo.Gameplay.SimpleUI
{
    public interface IUIService
    {
        UniTask ShowAsync<TPresenter>(bool immediately = false, CancellationToken ct = default)
            where TPresenter : WindowPresenter;

        UniTask HideAsync<TPresenter>(bool immediately = false, CancellationToken ct = default)
            where TPresenter : WindowPresenter;
    }
}