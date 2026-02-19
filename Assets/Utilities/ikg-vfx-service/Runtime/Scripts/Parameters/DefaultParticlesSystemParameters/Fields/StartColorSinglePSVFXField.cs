using System;
using IKGTools.VFXs.ParticlesSystem;
using UnityEngine;

namespace IKGTools.VFXs.DefaultParticlesSystemParameters.Fields
{
    [Serializable]
    public sealed class StartColorSinglePSVFXField : PSVFXParameterField
    {
        [SerializeField] private Color _color;

        public StartColorSinglePSVFXField(Color color)
        {
            _color = color;
        }

        internal override void Setup(IParticlesSystemVFX component)
        {
            var mainModule = component.ParticleSystems.main;
            mainModule.startColor = _color;
        }
    }
}