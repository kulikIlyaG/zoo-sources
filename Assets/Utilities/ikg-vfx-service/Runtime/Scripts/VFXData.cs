using UnityEngine;

namespace IKGTools.VFXs
{
    [CreateAssetMenu(fileName = "VFXData", menuName = "IKGTools/VFXs/Data")]
    public sealed class VFXData : ScriptableObject
    {
        [SerializeField] private VFXComponent _source;
        public string Id => name;
        public VFXComponent Source => _source;
    }
}