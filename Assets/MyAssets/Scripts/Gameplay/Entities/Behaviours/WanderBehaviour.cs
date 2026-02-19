using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Zoo.Gameplay.Environment;

namespace Zoo.Gameplay.Entities.Behaviours
{
    internal sealed class WanderBehaviour : SimpleBehaviour<WanderBehaviourParameters>
    {
        private IGameFieldPointProvider _gameFieldController;
        private IViewBoundsField _viewBoundsField;
        private Vector3 _targetPoint;
        private bool _hasTarget;
        
        private CancellationTokenSource _moveCts;
        private bool _isMoveInProgress;
        
        public WanderBehaviour(IEntity root) : base(root) {}
        
        protected override void ResolveReferences(IObjectResolver resolver)
        {
            _gameFieldController = resolver.Resolve<IGameFieldPointProvider>();
            _viewBoundsField = resolver.Resolve<IViewBoundsField>();
        }

        public override void OnSleepRequested()
        {
            StopMovement();
        }

        public override void Tick()
        {
            if (_isMoveInProgress)
            {
                return;
            }

            if (!_hasTarget)
            {
                _targetPoint = GetMoveTarget();
                _hasTarget = true;
            }

            MoveToTargetAsync(_targetPoint).Forget();
        }


        private Vector3 GetMoveTarget()
        {
            Vector3 current = _root.Position;
            if (_viewBoundsField.IsInBounds(current))
            {
                return _gameFieldController.GetPoint();
            }
            return _viewBoundsField.GetInBoundsPoint(_gameFieldController.GroundHeight, _gameFieldController.GetFieldRect());
        }

        private async UniTask MoveToTargetAsync(Vector3 targetPoint)
        {
            CancelCurrentMove();
            _moveCts = new CancellationTokenSource();
            CancellationToken token = _moveCts.Token;

            _isMoveInProgress = true;
            try
            {
                await _root.Movement.MoveToAsync(targetPoint, token);
            }
            finally
            {
                _hasTarget = false;
                _isMoveInProgress = false;
                _moveCts?.Dispose();
                _moveCts = null;
            }
        }

        public override void DeInitialize()
        {
            StopMovement();
        }

        private void StopMovement()
        {
            CancelCurrentMove();
            _hasTarget = false;
        }

        private void CancelCurrentMove()
        {
            if (_moveCts == null)
            {
                return;
            }

            if (!_moveCts.IsCancellationRequested)
            {
                _moveCts.Cancel();
            }
        }
    }

    [Serializable]
    public sealed class WanderBehaviourParameters : ISimpleBehaviourParameters{}

    [Serializable]
    public sealed class WanderBehaviourType : SimpleBehaviourType
    {
        public override ISimpleBehaviour CreateBehaviour(IEntity root)
        {
            return new WanderBehaviour(root);
        }
    }
}
