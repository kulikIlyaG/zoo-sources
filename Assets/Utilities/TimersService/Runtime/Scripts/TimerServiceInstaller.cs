using VContainer;
using VContainer.Unity;

namespace IKGTools.Services.Timers
{
    public static class TimerServiceInstaller
    {
        public static void Install(IContainerBuilder builder)
        {
            builder.Register<TimersService>(Lifetime.Singleton).As<ITimersService>().As<ITickable>();
            builder.Register<IntervalTimersService>(Lifetime.Singleton).As<IIntervalTimersService>();
        }
    }
}