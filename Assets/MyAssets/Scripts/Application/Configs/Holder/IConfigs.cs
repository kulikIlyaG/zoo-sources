namespace Zoo.Application.Configs
{
    public interface IConfigs
    {
        T GetConfig<T>() where T : class, IConfig;
    }
}