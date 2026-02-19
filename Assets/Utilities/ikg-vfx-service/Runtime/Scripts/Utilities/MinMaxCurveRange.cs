using System;
using UnityEngine;

namespace IKGTools.VFXs.Utilities
{
    [Serializable]
    public sealed class MinMaxCurveRange
    {
        [SerializeField] private ParticleSystem.MinMaxCurve _min;
        [SerializeField] private ParticleSystem.MinMaxCurve _max;

        public ParticleSystem.MinMaxCurve Min => _min;
        public ParticleSystem.MinMaxCurve Max => _max;

        public ParticleSystem.MinMaxCurve Blend(float t)
        {
            return _min.Blend(_max, t);
        }
    }
}