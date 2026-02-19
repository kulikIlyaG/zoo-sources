using IKGTools.VFXs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace IKGTools.VContainerSupport.VFXs
{
    internal sealed class VFXsServiceObjectsCreatorVContainer : VFXsServiceInstancesCreator
    {
        [Inject] private IObjectResolver _resolver;
        
        public override T CreateInstance<T>(string name, Vector3 position, Transform parent)
        {
            var instance = new GameObject(name);
            instance.transform.SetParent(parent);
            instance.transform.position = position;
            var component = instance.AddComponent<T>();
            
            _resolver.InjectGameObject(instance);
            
            return component;
        }

        public override T CreateInstance<T>(T source, string name, Vector3 position, Transform parent)
        {
            var instance = Instantiate(source, position, Quaternion.identity, parent);
            instance.name = name;

            _resolver.InjectGameObject(instance.gameObject);
            
            return instance;
        }
    }
}