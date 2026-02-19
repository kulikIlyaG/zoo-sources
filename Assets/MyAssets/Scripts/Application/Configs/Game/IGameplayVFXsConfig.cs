using IKGTools.VFXs;

namespace Zoo.Application.Configs
{
    public interface IGameplayVFXsConfig : IConfig
    {
        VFXData VFXDataAfterKill { get; }
    }
}