using IKGTools.VFXs.DefaultParticlesSystemParameters.Fields;
using IKGTools.VFXs.ParticlesSystem;
using UnityEngine;

namespace IKGTools.VFXs.DefaultParticlesSystemParameters
{
    public sealed class ParticlesSystemVFXParameters : VFXParameters
    {
        private readonly PSVFXParameterField[] _fields;

        public ParticlesSystemVFXParameters(PSVFXParameterField[] fields, Vector3? worldPosition, Vector3? localScale, Quaternion? rotation) : base(worldPosition, localScale, rotation)
        {
            _fields = fields;
        }

        public ParticlesSystemVFXParameters(PSVFXParameterField[] fields)
        {
            _fields = fields;
        }

        internal void Setup(VFXParticlesSystemComponent component)
        {
            foreach (PSVFXParameterField field in _fields)
            {
                field.Setup(component);
            }
        }
    }
}