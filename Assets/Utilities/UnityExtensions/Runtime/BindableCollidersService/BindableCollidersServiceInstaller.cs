using System;
using VContainer;

namespace Utilities.UnityExtensions
{
    public static class BindableCollidersServiceInstaller
    {
        public static void Install(IContainerBuilder builder)
        {
            builder.Register<BindableCollidersService>(Lifetime.Singleton).As<IBindableCollidersService>().As<IDisposable>();
        }
    }
}