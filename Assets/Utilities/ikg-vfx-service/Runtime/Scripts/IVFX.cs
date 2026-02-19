using System;
using Cysharp.Threading.Tasks;

namespace IKGTools.VFXs
{
    internal interface IVFX
    {
        void Raise(VFXParameters parameters = null, Action onFinished = null);
        UniTask RaiseAsync(VFXParameters parameters = null);
    }
}