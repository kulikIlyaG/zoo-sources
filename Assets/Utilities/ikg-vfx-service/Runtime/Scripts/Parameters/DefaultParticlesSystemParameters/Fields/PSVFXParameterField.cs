using System;
using IKGTools.VFXs.ParticlesSystem;

namespace IKGTools.VFXs.DefaultParticlesSystemParameters.Fields
{
    [Serializable]
    public abstract class PSVFXParameterField
    {
        internal abstract void Setup(IParticlesSystemVFX component);
    }
}