using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Zoo.Gameplay.SimpleUI.Components.Animations
{
    public abstract class UIShowHideAnimationComponent : MonoBehaviour
    {
        public abstract UniTask PlayShowAnimationAsync(CancellationToken cancellationToken = default);
        public abstract UniTask PlayHideAnimationAsync(CancellationToken cancellationToken = default);

        public abstract void ResetToShown();
        public abstract void ResetToHidden();
    }
}