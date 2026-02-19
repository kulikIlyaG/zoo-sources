using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IKGTools.VFXs
{
    [Serializable]
    internal class LazyIterator
    {
        [SerializeField] [Tooltip("When equals zero will be actions count unlimited per frame")]
        private int _countActionPerIteration = 1;
        [SerializeField] private int _framesInIteration = 1;

        private int _currentActionsCountPerFrame = 0;
        private int _currentFramesCounter = 0;

        public void Reset()
        {
            _currentActionsCountPerFrame = 0;
            _currentFramesCounter = 0;
        }
            
        public async UniTask WaitInstanceIterationAsync()
        {
            if(_countActionPerIteration <= 0)
                return;
                
            _currentActionsCountPerFrame++;

            if (_currentActionsCountPerFrame > _countActionPerIteration)
            {
                while (_currentFramesCounter < _framesInIteration)
                {
                    await UniTask.NextFrame();
                    _currentFramesCounter++;
                }
                    
                Reset();
            }
        }
    }
}