using System;
using IKGTools.VFXs;
using UnityEngine;
using Utilities.UnityExtensions;
using Utilities.VContainerExtensions;
using VContainer;
using VContainer.Unity;
using Zoo.Gameplay.Entities;
using Zoo.Gameplay.Environment;
using Zoo.Gameplay.Session;
using Zoo.Gameplay.SimpleUI;

namespace Zoo.Gameplay
{
    internal sealed class GameplayScope : BaseScope
    {
        [SerializeField] private EntitiesController _entitiesController;
        [SerializeField] private GameFieldController _gameFieldController;
        [SerializeField] private ViewCameraBoundsField viewCameraBoundsField;
        [SerializeField] private VFXsService _vfxsService;

        [Header("UI")]
        [SerializeField] private WindowsController _windowsController;
        [SerializeField] private MainWindowView _mainWindowView;
        
        protected override void ConfigureProcess(IContainerBuilder builder)
        {
            builder.RegisterInstance(_vfxsService).As<IVFXsService>();
            builder.RegisterInstance(_entitiesController).As<IEntitiesController>().As<ITickable>();
            builder.RegisterInstance(_gameFieldController).As<IGameFieldPointProvider>();
            builder.RegisterInstance(viewCameraBoundsField).As<IViewBoundsField>();
            builder.Register<EntitiesSpawnSystem>(Lifetime.Singleton).As<IEntitiesSpawnSystem>().As<IDisposable>();
            builder.Register<GameSessionController>(Lifetime.Singleton).As<IGameSessionController>().As<IGameSessionControllerReadOnly>();
            builder.Register<GameSessionData>(Lifetime.Singleton).As<IGameSessionData>().As<IGameSessionDataReadOnly>();

            ConfigureUI(builder);
            
            BindableCollidersServiceInstaller.Install(builder);
            EntitiesContactsResolverInstaller.Install(builder);
        }

        private void ConfigureUI(IContainerBuilder builder)
        {
            //Это все по хорошему не стоит делать, лучше создавать окна по запросу. Но тут я решил сэкономить время
            builder.RegisterInstance(_mainWindowView);
            builder.Register<MainWindowModel>(Lifetime.Singleton);
            builder.Register<MainWindowPresenter>(Lifetime.Singleton).As<WindowPresenter>();

            
            builder.RegisterInstance(_windowsController).As<IUIService>();
        }
    }
}