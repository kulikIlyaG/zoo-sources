using System;
using System.Collections.Generic;
using UnityEngine;

namespace IKGTools.VFXs
{
    [CreateAssetMenu(fileName = "VFXsPreInitData", menuName = "IKGTools/VFXs/Pre Init Data")]
    public sealed class PreInitializeData : ScriptableObject
    {
        [Serializable]
        private class VFXPoolPreInstance
        {
            public VFXData VFX;
            public int InitializeCount;
        }

        [SerializeField] private VFXPoolPreInstance[] _preInstances;

        [SerializeField] private LazyIterator _lazyIterator;

        internal LazyIterator LazyIterator => _lazyIterator;

        public IReadOnlyCollection<(VFXData vfx, int preInitializeCount)> GetPreInitializeInstancesCount()
        {
            List<(VFXData vfx, int preInitializeCount)> result =
                new List<(VFXData vfx, int preInitializeCount)>(_preInstances.Length);

            foreach (VFXPoolPreInstance data in _preInstances)
            {
                result.Add((data.VFX, data.InitializeCount));
            }

            return result;
        }

        private void OnValidate()
        {
            //todo check duplicates in _preInstances;
        }
    }
}