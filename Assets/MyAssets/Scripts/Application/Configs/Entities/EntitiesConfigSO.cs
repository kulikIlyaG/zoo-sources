using System.Collections.Generic;
using UnityEngine;

namespace Zoo.Application.Configs
{
    [CreateAssetMenu(fileName = "EntitiesConfig", menuName = "Configs/Entities")]
    internal sealed class EntitiesConfigSO : ScriptableObject, IEntitiesConfig
    {
        [SerializeField] private EntityDescription[] _entities;
        
        private IReadOnlyDictionary<string, EntityDescription> _tempConfigDictionary;

        public IReadOnlyDictionary<string, EntityDescription> Collection
        {
            get
            {
                if (_tempConfigDictionary == null)
                {
                    InitializeDictionary();
                }
                
                return _tempConfigDictionary;
            }
        }

        private void InitializeDictionary()
        {
            Dictionary<string, EntityDescription> dic = new Dictionary<string, EntityDescription>(_entities.Length);
            
            foreach (var entity in _entities)
            {
                dic[entity.Id] = entity;
            }
            
            _tempConfigDictionary = dic;
        }
    }
}