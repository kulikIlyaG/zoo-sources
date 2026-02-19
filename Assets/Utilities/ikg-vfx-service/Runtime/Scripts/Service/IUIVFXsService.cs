using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IKGTools.VFXs.UI
{
    public interface IUIVFXsService
    {
        /// <summary>
        /// uiParent use for set correct parent in ui space
        /// </summary>
        void Raise(VFXData vfx, VFXParameters parameters, Transform uiParent);
        
        /// <summary>
        /// uiParent use for set correct parent in ui space
        /// </summary>
        UniTask RaiseAsync(VFXData vfx, VFXParameters parameters, Transform uiParent);
    }
}