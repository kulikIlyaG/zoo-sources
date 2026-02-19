using Cysharp.Threading.Tasks;
using IKGTools.VFXs.UI;

namespace IKGTools.VFXs
{
    public interface IVFXsService : IUIVFXsService
    {
        UniTask InitializeAsync(PreInitializeData preInitializeData = null);
        void Raise(VFXData vfx, VFXParameters parameters);
        UniTask RaiseAsync(VFXData vfx, VFXParameters parameters);
    }
}