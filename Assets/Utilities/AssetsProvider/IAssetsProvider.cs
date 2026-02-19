using System.Threading;
using Cysharp.Threading.Tasks;

namespace Utilities.AssetsProvider
{
    public interface IAssetsProvider
    {
        UniTask<T> GetAssetAsync<T>(string path, CancellationToken cancellationToken = default) where T : UnityEngine.Object;
        void ReleaseAsset<T>(string path) where T : UnityEngine.Object;
    }
}
