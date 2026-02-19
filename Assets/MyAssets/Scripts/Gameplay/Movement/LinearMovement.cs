using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Zoo.Gameplay.Movements
{
    internal sealed class LinearMovement : Movement<LinearMovementParameters>
    {
        public LinearMovement(LinearMovementParameters parameters, Rigidbody moveTarget) : base(parameters, moveTarget) {}

        public override async UniTask MoveToAsync(Vector3 point, CancellationToken cancellationToken = default)
        {
            float speed = Mathf.Max(0.01f, Parameters.Speed);
            const float stopDistance = 0.05f;
            float stopDistanceSqr = stopDistance * stopDistance;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Vector3 current = MoveTarget.position;
                Vector3 target = new Vector3(point.x, current.y, point.z);
                Vector3 toTarget = target - current;
                toTarget.y = 0f;

                if (toTarget.sqrMagnitude <= stopDistanceSqr)
                {
                    break;
                }

                Vector3 nextPosition = Vector3.MoveTowards(current, target, speed * Time.fixedDeltaTime);

                MoveTarget.MovePosition(nextPosition);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken);
            }

            MoveTarget.linearVelocity = Vector3.zero;
        }
    }

    [Serializable]
    public sealed class LinearMovementParameters : IMovementParameters
    {
        public float Speed = 2f;
    }
    
    [Serializable]
    public sealed class LinearMovementType : MovementType
    {
        [SerializeField] private LinearMovementParameters _parameters = new();


        public override IMovement CreateMovement(Rigidbody moveTarget)
        {
            return new LinearMovement(_parameters, moveTarget);
        }
    }
}
