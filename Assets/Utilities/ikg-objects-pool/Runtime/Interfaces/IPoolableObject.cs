namespace IKGTools.ObjectsPool
{
    public interface IPoolableObject
    {
        void OnAddedToPool();
        void OnRemovedFromPool();

        void OnGotIn();
        void OnGotOut();
    }
}