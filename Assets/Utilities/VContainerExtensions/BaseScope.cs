using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Utilities.VContainerExtensions
{
    [RequireComponent(typeof(ScopeEnterPoint))]
    public abstract class BaseScope : LifetimeScope
    {
        [SerializeField] private ScopeEnterPoint _scopeEnterPoint;

        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureProcess(builder);
            
            builder.RegisterInstance(_scopeEnterPoint).As<IAsyncStartable>();
        }

        protected abstract void ConfigureProcess(IContainerBuilder builder);
    }
}