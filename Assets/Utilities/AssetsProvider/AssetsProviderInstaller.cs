using System;
using VContainer;

namespace Utilities.AssetsProvider
{
    public static class AssetsProviderInstaller
    {
        public static void Install(IContainerBuilder builder)
        {
            builder.Register<AddressablesAssetsProvider>(Lifetime.Singleton).As<IAssetsProvider>().As<IDisposable>();
        }
    }
}
