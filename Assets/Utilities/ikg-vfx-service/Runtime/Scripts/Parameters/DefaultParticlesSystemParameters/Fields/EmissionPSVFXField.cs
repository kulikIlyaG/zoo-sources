using System;
using IKGTools.VFXs.ParticlesSystem;
using UnityEngine;

namespace IKGTools.VFXs.DefaultParticlesSystemParameters.Fields
{
    [Serializable]
    public sealed class EmissionPSVFXField : PSVFXParameterField
    {   
        [SerializeField] private ParticleSystem.MinMaxCurve _curve = 10;

        public EmissionPSVFXField(ParticleSystem.MinMaxCurve curve)
        {
            _curve = curve;
        }

        internal override void Setup(IParticlesSystemVFX component)
        {
            var module = component.ParticleSystems.emission;
            module.rateOverTime = _curve;
        }
    }
}