using System;
using System.Collections.Generic;
using System.Linq;

namespace IKGTools.ObjectsPool
{
    [Serializable]
    public class Pool<T> : IPool<T> where T : IPoolableObject
    {
        private readonly HashSet<T> _all;
        private readonly HashSet<T> _inside; 
        private readonly HashSet<T> _outside;
        
        public int Count => _all.Count;
        
        public Pool(int capacity)
        {
            _all = new HashSet<T>(capacity);
            _inside = new HashSet<T>(capacity);
            _outside = new HashSet<T>(capacity);
        }

        public int FreeObjectsCount => _inside.Count;

        public void Add(T obj)
        {
            _all.Add(obj);
            _inside.Add(obj);
            obj.OnAddedToPool();
        }

        public void Remove(T obj)
        {
            if (!_inside.Contains(obj))
            {
                throw new Exception($"You can't remove obj({obj.ToString()}) from pool while he used; You need release obj before remove");
            }
            
            _all.Remove(obj);
            _outside.Remove(obj);
            obj.OnRemovedFromPool();
        }

        public T Take()
        {
            if (_inside is {Count: 0})
            {
                throw new Exception($"You trying to get obj from pool({typeof(T).Name}). But this pool don't has a free object!");
            }

            var obj = _inside.First();
            _inside.Remove(obj);
            _outside.Add(obj);
            obj.OnGotOut();
            return obj;
        }

        public void Release(T obj)
        {
            _outside.Remove(obj);
            _inside.Add(obj);
            obj.OnGotIn();
        }
    }
}