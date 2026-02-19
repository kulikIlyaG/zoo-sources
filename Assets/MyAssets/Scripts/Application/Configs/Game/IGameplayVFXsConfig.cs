using IKGTools.VFXs;
using Zoo.Application.Configs;

namespace MyAssets.Scripts.Application.Configs.Game
{
    public interface IGameplayVFXsConfig : IConfig
    {
        VFXData VFXDataAfterKill { get; }
    }
}