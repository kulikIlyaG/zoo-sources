using System;

namespace IKGTools.Services.Timers
{
    public interface ITimersService
    {
        ITimerReadOnly CreateTimer(float duration);
        ushort CreateTimer(float duration, Action onFinish);
        
        void KillTimer(ushort timerId);

        void PauseTimer(ushort timerId);
        void ResumeTimer(ushort timerId);
        
        bool IsTimerRunning(ushort timerId);
        ITimerReadOnly GetTimer(ushort timerId);
    }
}