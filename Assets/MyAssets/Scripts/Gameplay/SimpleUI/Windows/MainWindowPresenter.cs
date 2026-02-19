using System.Threading;
using Cysharp.Threading.Tasks;
using Zoo.Gameplay.Session;

namespace Zoo.Gameplay.SimpleUI
{
    public sealed class MainWindowPresenter : WindowPresenter<MainWindowModel, MainWindowView>
    {
        private readonly IGameSessionDataReadOnly _gameSessionData;
        
        public MainWindowPresenter(MainWindowModel model, MainWindowView view, IGameSessionDataReadOnly gameSessionData) : base(model, view)
        {
            _gameSessionData = gameSessionData;
        }

        protected override UniTask BeforeShowAsync(bool immediately, CancellationToken cancellationToken)
        {
            UpdateModel();
            _view.SetPreysCounter(_model.KilledPreys);
            _view.SetPredatorsCounter(_model.KilledPredators);
            
            return base.BeforeShowAsync(immediately, cancellationToken);
        }

        private void UpdateModel()
        {
            _model.KilledPreys = _gameSessionData.TotalKilledPreys;
            _model.KilledPredators = _gameSessionData.TotalKilledPredators;
        }

        private void OnKilledPrey()
        {
            _model.KilledPreys++;
            _view.SetPreysCounter(_model.KilledPreys);
        }

        private void OnKilledPredator()
        {
            _model.KilledPredators++;
            _view.SetPredatorsCounter(_model.KilledPredators);
        }

        protected override void SubscribeToEvents()
        {
            _gameSessionData.OnChangedTotalKilledPreys += OnKilledPrey;
            _gameSessionData.OnChangedTotalKilledPredators += OnKilledPredator;
        }

        protected override void UnsubscribeFromEvents()
        {
            _gameSessionData.OnChangedTotalKilledPreys -= OnKilledPrey;
            _gameSessionData.OnChangedTotalKilledPredators -= OnKilledPredator;
        }
    }
}