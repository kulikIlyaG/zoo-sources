using System;
using Cysharp.Threading.Tasks;
using IKGTools.VFXs.DefaultParticlesSystemParameters;
using UnityEngine;

namespace IKGTools.VFXs.ParticlesSystem
{
    public class VFXParticlesSystemComponent : VFXComponent, IParticlesSystemVFX
    {
        [SerializeField] protected ParticleSystem _particle;
        ParticleSystem IParticlesSystemVFX.ParticleSystems => _particle;
        
        protected override async void Raise(Action onFinished)
        {
            await RaiseAndWaitAsync();
            onFinished?.Invoke();
        }

        protected override async UniTask RaiseAsync()
        {
            await RaiseAndWaitAsync();
        }

        protected override void SetupParameters(VFXParameters baseParameters)
        {
            base.SetupParameters(baseParameters);

            if (baseParameters is ParticlesSystemVFXParameters parameters)
                parameters.Setup(this);
        }

        private async UniTask RaiseAndWaitAsync()
        {
            SetActiveAndPlay();
            
            await WaitPlayingAsync();
        }

        protected virtual async UniTask WaitPlayingAsync()
        {
            await UniTask.WaitWhile(() => _particle.isPlaying);
        }

        protected virtual void SetActiveAndPlay()
        {
            if(!_particle.gameObject.activeSelf)
                _particle.gameObject.SetActive(true);
            
            if(!_particle.main.playOnAwake)
                _particle.Play(true);
        }

    }
}