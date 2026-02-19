using System;
using Cysharp.Threading.Tasks;

namespace IKGTools.Services.Timers
{
    internal interface ITimer : ITimerReadOnly
    {
        void Stop();
        void Resume();
        void Pause();
    }

    public interface ITimerReadOnly
    {
        ushort Id { get; }
        TimeSpan TotalTime { get; }
        TimeSpan Elapsed { get; }
        UniTask Await();
        event Action OnPause;
        event Action OnResume;
        event Action OnKill;
        event Action OnFinish;
    }
}