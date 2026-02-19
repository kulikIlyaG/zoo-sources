using System;
using System.Collections.Generic;

namespace IKGTools.Services.Timers
{
    internal sealed class IntervalTimersService : IIntervalTimersService
    {
        private readonly ITimersService _timersService;

        private readonly Dictionary<ushort, ushort> _intervalTimers = new();
        private ushort _nextIntervalId;

        public IntervalTimersService(ITimersService timersService)
        {
            _timersService = timersService;
        }

        public ushort CreateInterval(float interval, Action<TimeSpan> onTick, Action onFinish = null, int repeatCount = 0)
        {
            if (interval <= 0)
                throw new ArgumentException("Interval must be greater than 0", nameof(interval));

            var intervalId = ++_nextIntervalId;
            int executedCount = 0;

            Action callback = null;
            callback = () =>
            {
                executedCount++;
                onTick?.Invoke(TimeSpan.FromSeconds(interval * executedCount));

                if (repeatCount > 0 && executedCount >= repeatCount)
                {
                    onFinish?.Invoke();
                    KillInterval(intervalId);
                }
                else
                {
                    var newTimerId = _timersService.CreateTimer(interval, callback);
                    _intervalTimers[intervalId] = newTimerId;
                }
            };

            var timerId = _timersService.CreateTimer(interval, callback);
            _intervalTimers[intervalId] = timerId;

            return intervalId;
        }

        public void KillInterval(ushort intervalId)
        {
            if (_intervalTimers.TryGetValue(intervalId, out var timerId))
            {
                _timersService.KillTimer(timerId);
                _intervalTimers.Remove(intervalId);
            }
        }

        public void PauseInterval(ushort intervalId)
        {
            if (_intervalTimers.TryGetValue(intervalId, out var timerId))
            {
                _timersService.PauseTimer(timerId);
            }
        }

        public void ResumeInterval(ushort intervalId)
        {
            if (_intervalTimers.TryGetValue(intervalId, out var timerId))
            {
                _timersService.ResumeTimer(timerId);
            }
        }
    }
}