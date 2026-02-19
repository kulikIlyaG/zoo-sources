using System.Threading;
using Cysharp.Threading.Tasks;
using Utilities.VContainerExtensions;

namespace Zoo.Application
{
    internal sealed class ApplicationScopeEnterPoint : ScopeEnterPoint
    {
        protected override UniTask ExecuteEnterPointAsync(CancellationToken cancellationToken)
        {
            return default;
        }
    }
}