using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Zoo.Gameplay.SimpleUI
{
    public abstract class WindowPresenter
    {
        private enum WindowState
        {
            Hidden = 0,
            Showing = 1,
            Shown = 2,
            Hiding = 3
        }

        private bool _isInitialized = false;
        private WindowState _state = WindowState.Hidden;
        private CancellationTokenSource _transitionCts;
        
        private async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            if (_isInitialized)
            {
                throw new Exception($"Window({this.GetType().Name}) is already initialized!");
            }
            
            await InitializeProcessAsync(cancellationToken);
            _isInitialized = true;
        }
        protected virtual UniTask InitializeProcessAsync(CancellationToken cancellationToken) => default;
        
        
        public async UniTask ShowAsync(bool immediately = false, CancellationToken cancellationToken = default)
        {
            if (_state is WindowState.Shown or WindowState.Showing)
            {
                return;
            }

            if (!_isInitialized)
            {
                await InitializeAsync(cancellationToken);
            }

            CancellationToken token = CreateTransitionToken(cancellationToken, out CancellationTokenSource linkedCts);
            _state = WindowState.Showing;
            try
            {
                await BeforeShowAsync(immediately, token);
                await ShowProcessAsync(immediately, token);
                SubscribeToEvents();
                OnShow();
                _state = WindowState.Shown;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                _state = WindowState.Hidden;
                throw;
            }
            finally
            {
                linkedCts.Dispose();
            }
        }

        protected virtual void SubscribeToEvents(){}
        protected virtual void UnsubscribeFromEvents(){}


        public async UniTask HideAsync(bool immediately = false, CancellationToken cancellationToken = default)
        {
            if (_state is WindowState.Hidden or WindowState.Hiding)
            {
                return;
            }

            CancellationToken token = CreateTransitionToken(cancellationToken, out CancellationTokenSource linkedCts);
            _state = WindowState.Hiding;
            try
            {
                BeforeHide(immediately);
                await HideProcessAsync(immediately, token);
                UnsubscribeFromEvents();
                OnHidden();
                _state = WindowState.Hidden;
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                _state = WindowState.Shown;
                throw;
            }
            finally
            {
                linkedCts.Dispose();
            }
        }
        

        protected virtual UniTask BeforeShowAsync(bool immediately, CancellationToken cancellationToken) => default;
        protected abstract UniTask ShowProcessAsync(bool immediately, CancellationToken cancellationToken);
        protected virtual void OnShow(){}


        protected virtual void BeforeHide(bool immediately) {}
        protected abstract UniTask HideProcessAsync(bool immediately, CancellationToken cancellationToken);
        protected virtual void OnHidden() {}

        private CancellationToken CreateTransitionToken(CancellationToken externalToken, out CancellationTokenSource linkedCts)
        {
            _transitionCts?.Cancel();
            _transitionCts?.Dispose();
            _transitionCts = new CancellationTokenSource();

            linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalToken, _transitionCts.Token);
            return linkedCts.Token;
        }
    }
    
    public abstract class WindowPresenter<TModel, TView> : WindowPresenter 
        where TModel : class, IWindowModel
        where TView : class, IWindowView
    {
        protected readonly TModel _model;
        protected readonly TView _view;

        protected WindowPresenter(TModel model, TView view)
        {
            _model = model;
            _view = view;
        }

        protected override async UniTask ShowProcessAsync(bool immediately, CancellationToken cancellationToken)
        {
            await _view.ShowAsync(immediately, cancellationToken);
        }

        protected override async UniTask HideProcessAsync(bool immediately, CancellationToken cancellationToken)
        {
            await _view.HideAsync(immediately, cancellationToken);
        }
    }
}
