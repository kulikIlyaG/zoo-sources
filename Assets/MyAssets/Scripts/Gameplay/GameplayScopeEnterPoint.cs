using System.Threading;
using Cysharp.Threading.Tasks;
using IKGTools.VFXs;
using Utilities.VContainerExtensions;
using VContainer;
using Zoo.Gameplay.Session;
using Zoo.Gameplay.SimpleUI;

namespace Zoo.Gameplay
{
    internal sealed class GameplayScopeEnterPoint : ScopeEnterPoint
    {
        private IGameSessionController _gameSessionController;
        private IUIService _uiService;
        private IVFXsService _vfxsService;

        [Inject]
        private void Construct(IGameSessionController gameSessionController, IUIService uiService, IVFXsService vfxsService)
        {
            _gameSessionController = gameSessionController;
            _uiService = uiService;
            _vfxsService = vfxsService;
        }
        
        protected override async UniTask ExecuteEnterPointAsync(CancellationToken cancellationToken)
        {
            await _vfxsService.InitializeAsync();
            await _uiService.ShowAsync<MainWindowPresenter>(ct: cancellationToken);
            _gameSessionController.StartGame();
        }
    }
}