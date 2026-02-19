
using System;
using UnityEngine;

namespace IKGTools.VFXs.SubElements.Parameters
{
    public sealed class VFXSubElementTransformRefParameter : SubVFXElementParameter
    {
        public VFXSubElementTransformRefParameter(string id, Transform value) : base(id)
        {
            Value = value;
        }

        public VFXSubElementTransformRefParameter(Transform value) : base(String.Empty)
        {
            Value = value;
        }
        public Transform Value { get; }
    }
}