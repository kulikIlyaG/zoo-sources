using IKGTools.VFXs;
using UnityEngine;

namespace MyAssets.Scripts.Application.Configs.Game
{
    [CreateAssetMenu(fileName = "GameplayVFXsConfig", menuName = "Configs/VFXs")]
    internal sealed class GameplayVFXsConfigSO : ScriptableObject, IGameplayVFXsConfig
    {
        [SerializeField] private VFXData _vfxDataAfterKill;
        
        public VFXData VFXDataAfterKill => _vfxDataAfterKill;
    }
}