using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zoo.Gameplay.SimpleUI.Components.Animations;

namespace Zoo.Gameplay.SimpleUI
{
    public abstract class WindowView : MonoBehaviour, IWindowView
    {
        [SerializeField] private UIShowHideAnimationComponent _showHideAnimation;
        
        public async UniTask ShowAsync(bool immediately = false, CancellationToken cancellationToken = default)
        {
            if (_showHideAnimation != null)
            {
                _showHideAnimation.ResetToHidden();
            }
            
            await BeforeShowAsync(immediately, cancellationToken);
            
            gameObject.SetActive(true);
            
            if (!immediately)
            {
                if (_showHideAnimation != null)
                {
                    await _showHideAnimation.PlayShowAnimationAsync(cancellationToken);
                }
            }
        }

        protected virtual UniTask BeforeShowAsync(bool immediately, CancellationToken cancellationToken) => default;

        public async UniTask HideAsync(bool immediately = false, CancellationToken cancellationToken = default)
        {
            await BeforeHideAsync(immediately, cancellationToken);
            
            if (_showHideAnimation != null)
            {
                _showHideAnimation.ResetToShown();

                if (!immediately)
                {
                    await _showHideAnimation.PlayHideAnimationAsync(cancellationToken);
                }
            }

            gameObject.SetActive(false);
        }

        protected virtual UniTask BeforeHideAsync(bool immediately, CancellationToken cancellationToken) => default;
    }
    
    public interface IWindowView
    {
        UniTask ShowAsync(bool immediately = false, CancellationToken cancellationToken = default);
        UniTask HideAsync(bool immediately = false, CancellationToken cancellationToken = default);
    }
}