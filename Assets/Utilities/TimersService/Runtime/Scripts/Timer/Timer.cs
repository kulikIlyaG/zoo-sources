using System;
using Cysharp.Threading.Tasks;

namespace IKGTools.Services.Timers
{
    internal sealed class Timer : ITimer
    {
        private bool _isPaused = false;

        private float _elapsed = 0f;

        public ushort Id { get; }
        public TimeSpan TotalTime { get; }
        public TimeSpan Elapsed => TimeSpan.FromSeconds(_elapsed);

        public event Action OnPause;
        public event Action OnResume;
        public event Action OnKill;
        public event Action OnFinish;

        private readonly UniTaskCompletionSource _awaitSource = new();

        public Timer(ushort id, float durationSeconds)
        {
            Id = id;
            TotalTime = TimeSpan.FromSeconds(durationSeconds);
        }

        
        public bool Tick(float delta)
        {
            if (_isPaused)
                return false;

            _elapsed += delta;

            if (_elapsed >= TotalTime.TotalSeconds)
            {
                OnFinish?.Invoke();
                _awaitSource.TrySetResult();
                return true;
            }

            return false;
        }

        public UniTask Await() => _awaitSource.Task;

        public void Pause()
        {
            if (_isPaused) return;
            _isPaused = true;
            OnPause?.Invoke();
        }

        public void Resume()
        {
            if (!_isPaused) return;
            _isPaused = false;
            OnResume?.Invoke();
        }

        public void Stop()
        {
            OnKill?.Invoke();
            _awaitSource.TrySetCanceled();
        }
    }
}