using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace IKGTools.Services.Timers
{
    internal sealed class TimersService : ITimersService, ITickable
    {
        private readonly Dictionary<ushort, ITimer> _timers = new();
        private ushort _nextId;
        private HashSet<ITimer> _createdTimers = new();
        private HashSet<ushort> _removedTimers = new();

        public ITimerReadOnly CreateTimer(float duration)
        {
            var timer = CreateInternal(duration);
            return timer;
        }

        public ushort CreateTimer(float duration, Action onFinish)
        {
            var timer = CreateInternal(duration);
            timer.OnFinish += onFinish;
            return timer.Id;
        }

        public void KillTimer(ushort timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.Stop();
                _removedTimers.Add(timerId);
            }
        }

        public void PauseTimer(ushort timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.Pause();
            }
        }

        public void ResumeTimer(ushort timerId)
        {
            if (_timers.TryGetValue(timerId, out var timer))
            {
                timer.Resume();
            }
        }

        public bool IsTimerRunning(ushort timerId)
        {
            return _timers.ContainsKey(timerId);
        }

        public ITimerReadOnly GetTimer(ushort timerId)
        {
            return _timers[timerId];
        }

        public void Tick()
        {
            var completed = new HashSet<ushort>(_removedTimers);

            foreach (var kvp in _timers)
            {
                if (kvp.Value is Timer timer && timer.Tick(Time.deltaTime))
                {
                    completed.Add(kvp.Key);
                }
            }

            foreach (var id in completed)
            {
                _timers.Remove(id);
            }

            foreach (ITimer timer in _createdTimers)
            {
                _timers.Add(timer.Id, timer);
            }
            
            _removedTimers.Clear();
            _createdTimers.Clear();
        }

        private Timer CreateInternal(float duration)
        {
            var id = ++_nextId;
            var timer = new Timer(id, duration);
            _createdTimers.Add(timer);
            return timer;
        }
    }
}