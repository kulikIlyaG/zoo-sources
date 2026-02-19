using UnityEngine;

namespace IKGTools.VFXs
{
    public abstract class VFXsServiceInstancesCreator : MonoBehaviour
    {
        public abstract T CreateInstance<T>(string name, Vector3 position, Transform parent) where T : MonoBehaviour;

        public abstract T CreateInstance<T>(T source, string name, Vector3 position, Transform parent) where T : MonoBehaviour;
    }
}