using IKGTools.VFXs;
using MyAssets.Scripts.Application.Configs.Game;
using UnityEngine;
using Zoo.Application.Configs;
using Zoo.Gameplay.Session;

namespace Zoo.Gameplay.Entities
{
    internal sealed class ContactActionKillEntity : ContactAction
    {
        private readonly IGameSessionController _gameSessionController;
        private readonly IVFXsService _vfxsService;
        
        private readonly VFXData _vfxDataAfterKill;

        public ContactActionKillEntity(IGameSessionController gameSessionController, IVFXsService vfxsService, IConfigs configs) : base(

            ContactTypeId.Of<PredatorContactWithPrey>(),
            ContactTypeId.Of<PredatorContactWithPredator>()

        )
        {
            _gameSessionController = gameSessionController;
            _vfxsService = vfxsService;

            _vfxDataAfterKill = configs.GetConfig<IGameplayVFXsConfig>().VFXDataAfterKill;
        }

        public override void Execute(ContactData contactData)
        {
            _gameSessionController.KillEntity(contactData.A, contactData.B);

            if (_vfxDataAfterKill != null)
            {
                Vector3 vfxPos = new Vector3(contactData.A.Position.x, 0f, contactData.A.Position.z);
                _vfxsService.Raise(_vfxDataAfterKill, new VFXParameters {WorldPosition = vfxPos});
            }
        }
    }
}