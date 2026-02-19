using IKGTools.VFXs;
using UnityEngine;

namespace Zoo.Application.Configs
{
    [CreateAssetMenu(fileName = "GameplayVFXsConfig", menuName = "Configs/VFXs")]
    internal sealed class GameplayVFXsConfigSO : ScriptableObject, IGameplayVFXsConfig
    {
        [SerializeField] private VFXData _vfxDataAfterKill;
        
        public VFXData VFXDataAfterKill => _vfxDataAfterKill;
    }
}