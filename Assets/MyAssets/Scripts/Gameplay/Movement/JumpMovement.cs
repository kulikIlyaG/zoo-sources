using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Zoo.Gameplay.Movements
{
    internal sealed class JumpMovement : Movement<JumpMovementParameters>
    {
        public JumpMovement(JumpMovementParameters parameters, Rigidbody moveTarget) : base(parameters, moveTarget) {}

        public override async UniTask MoveToAsync(Vector3 point, CancellationToken cancellationToken = default)
        {
            if (MoveTarget == null)
            {
                return;
            }

            Vector3 start = MoveTarget.position;
            Vector3 end = new Vector3(point.x, start.y, point.z);

            float totalDistance = Vector3.Distance(start, end);
            float maxJumpLength = Mathf.Max(0.01f, Parameters.MaxJumpLength);
            int jumpsCount = Mathf.Max(1, Mathf.CeilToInt(totalDistance / maxJumpLength));

            Vector3 jumpStart = start;
            for (int jumpIndex = 1; jumpIndex <= jumpsCount; jumpIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (MoveTarget == null)
                {
                    return;
                }

                float delayBetweenJumps = Mathf.Max(0f, Parameters.DelayBetweenJumps);
                if (delayBetweenJumps > 0f)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delayBetweenJumps), cancellationToken: cancellationToken);
                }

                float normalized = (float)jumpIndex / jumpsCount;
                Vector3 jumpEnd = Vector3.Lerp(start, end, normalized);
                await PerformJumpAsync(jumpStart, jumpEnd, cancellationToken);

                jumpStart = jumpEnd;
            }

            if (MoveTarget != null)
            {
                MoveTarget.MovePosition(end);
                MoveTarget.linearVelocity = Vector3.zero;
            }
        }

        private async UniTask PerformJumpAsync(Vector3 start, Vector3 end, CancellationToken cancellationToken)
        {
            float distance = Vector3.Distance(start, end);
            float speed = Mathf.Max(0.01f, Parameters.JumpSpeed);
            float duration = Mathf.Max(0.05f, distance / speed);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (MoveTarget == null)
                {
                    return;
                }

                elapsed += Time.fixedDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                Vector3 horizontal = Vector3.Lerp(start, end, t);
                float arc = 4f * Parameters.JumpHeight * t * (1f - t);
                horizontal.y += arc;

                MoveTarget.MovePosition(horizontal);

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken);
            }
        }
    }

    [Serializable]
    public sealed class JumpMovementParameters : IMovementParameters
    {
        public float JumpSpeed = 1f;
        public float JumpHeight = 0.75f;
        public float MaxJumpLength = 0.75f;
        public float DelayBetweenJumps = 0.5f;
    }
    
    [Serializable]
    public sealed class JumpMovementType : MovementType
    {
        [SerializeField] private JumpMovementParameters _parameters = new();


        public override IMovement CreateMovement(Rigidbody moveTarget)
        {
            return new JumpMovement(_parameters, moveTarget);
        }
    }
}
