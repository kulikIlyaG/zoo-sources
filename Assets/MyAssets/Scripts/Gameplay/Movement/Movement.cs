using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Zoo.Gameplay.Movements
{
    public interface IMovement
    {
        public UniTask MoveToAsync(Vector3 point, CancellationToken cancellationToken = default);
    }
    
    [Serializable]
    public abstract class MovementType
    {
        public abstract IMovement CreateMovement(Rigidbody moveTarget);
    }

    public interface IMovementParameters {}

    internal abstract class Movement<TParameters> : IMovement
        where TParameters : class, IMovementParameters
    {
        protected readonly Rigidbody MoveTarget;
        protected readonly TParameters Parameters;

        protected Movement(TParameters parameters, Rigidbody moveTarget)
        {
            Parameters = parameters;
            MoveTarget = moveTarget;
        }

        public abstract UniTask MoveToAsync(Vector3 point, CancellationToken cancellationToken = default);
    }
}
