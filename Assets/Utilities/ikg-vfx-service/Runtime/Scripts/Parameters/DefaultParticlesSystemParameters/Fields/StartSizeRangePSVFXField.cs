using IKGTools.VFXs.ParticlesSystem;
using UnityEngine;

namespace IKGTools.VFXs.DefaultParticlesSystemParameters.Fields
{
    public class StartSizeRangePSVFXField : PSVFXParameterField
    {
        private readonly ParticleSystem.MinMaxCurve _curve;

        public StartSizeRangePSVFXField(ParticleSystem.MinMaxCurve curve)
        {
            _curve = curve;
        }

        internal override void Setup(IParticlesSystemVFX component)
        {
            var s = component.ParticleSystems.main;
            s.startSize = _curve;
        }
    }
}