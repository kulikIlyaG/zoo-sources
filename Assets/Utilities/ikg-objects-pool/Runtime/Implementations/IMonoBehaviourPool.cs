using UnityEngine;

namespace IKGTools.ObjectsPool
{
    public interface IMonoBehaviourPool<T> : IPool<T> where T : IPoolableObject
    { 
        T Take(Transform customParent);
    }
}