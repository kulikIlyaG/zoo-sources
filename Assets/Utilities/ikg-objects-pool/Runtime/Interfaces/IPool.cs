namespace IKGTools.ObjectsPool
{
    public interface IPool<T> where T : IPoolableObject
    {
        int FreeObjectsCount { get; }
        int Count { get; }
        
        void Add(T obj);
        void Remove(T obj);
        T Take();
        void Release(T obj);
    }
}