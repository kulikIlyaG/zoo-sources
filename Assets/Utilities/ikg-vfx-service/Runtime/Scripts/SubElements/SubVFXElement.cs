using UnityEngine;

namespace IKGTools.VFXs.SubElements
{
    public abstract class SubVFXElement : MonoBehaviour
    {
        [SerializeField] private string _parameterId;
        
        public void SetParameters(SubVFXElementParameter[] parameters)
        {
            if (_parameterId != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Id.Equals(_parameterId) || string.IsNullOrEmpty(parameter.Id))
                        ApplyParameters(parameter);
                }
            }
            else
            {
                foreach (var parameter in parameters)
                {
                    ApplyParameters(parameter);
                }
            }
        }

        protected abstract void ApplyParameters(SubVFXElementParameter parameter);
    }
}