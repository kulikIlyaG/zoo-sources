using UnityEngine;

namespace Utilities.UnityExtensions
{
    public sealed class SReferenceView : PropertyAttribute
    {
        public bool IncludeBaseType { get; }

        public SReferenceView(bool includeBaseType = false)
        {
            IncludeBaseType = includeBaseType;
        }
    }
}
