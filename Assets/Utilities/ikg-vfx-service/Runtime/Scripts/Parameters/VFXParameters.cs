using IKGTools.VFXs.SubElements;
using UnityEngine;

namespace IKGTools.VFXs
{
    public class VFXParameters
    {
        public Vector3? WorldPosition;
        public Vector3? LocalScale;
        public Quaternion? Rotation;

        public SubVFXElementParameter[] SubElements;

        public VFXParameters(Vector3? worldPosition, Vector3? localScale, Quaternion? rotation)
        {
            WorldPosition = worldPosition;
            LocalScale = localScale;
            Rotation = rotation;
        }

        public VFXParameters()
        {
        }
    }
}