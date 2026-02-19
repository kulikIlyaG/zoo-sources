using UnityEngine;

namespace IKGTools.VFXs.Utilities
{
    public static class MinMaxCurveExtension
    {
        public static ParticleSystem.MinMaxCurve Blend(this ParticleSystem.MinMaxCurve a, ParticleSystem.MinMaxCurve b,
            float percent)
        {
            float minCurve = Mathf.Lerp(a.constantMin, b.constantMin, percent);
                    
            float maxCurve = Mathf.Lerp(a.constantMax, b.constantMax, percent);

            if (maxCurve < minCurve)
                maxCurve = minCurve;

            return new ParticleSystem.MinMaxCurve(minCurve, maxCurve);
        }
    }
}