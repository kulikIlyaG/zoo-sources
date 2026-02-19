using System;

namespace IKGTools.Services.Timers
{
    public interface IIntervalTimersService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="onTick"></param>
        /// <param name="onFinish"></param>
        /// <param name="repeatCount">0 is infinity</param>
        /// <returns></returns>
        ushort CreateInterval(float interval, Action<TimeSpan> onTick, Action onFinish = null, int repeatCount = 0);
        void KillInterval(ushort intervalId);
        void PauseInterval(ushort intervalId);
        void ResumeInterval(ushort intervalId);
    }
}