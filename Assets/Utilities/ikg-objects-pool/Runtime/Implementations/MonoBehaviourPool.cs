using UnityEngine;

namespace IKGTools.ObjectsPool
{
    public class MonoBehaviourPool<T> : MonoBehaviour, IMonoBehaviourPool<T> where T : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private Transform _insideParent;
        [SerializeField] private Transform _outsideParent;
        
        [SerializeField] private Pool<T> _pool;

        public int FreeObjectsCount => _pool.FreeObjectsCount;
        public int Count => _pool.Count;

        protected virtual void Awake()
        {
            ValidateParents();
        }

        public void InitializePool(int capacity = 100)
        {
            _pool = new Pool<T>(capacity);
        }

        public void Add(T obj)
        {
            obj.transform.SetParent(_insideParent);
            _pool.Add(obj);
        }

        public void Remove(T obj)
        {
            _pool.Remove(obj);
        }

        public T Take()
        {
            var obj = _pool.Take();
            
            obj.transform.SetParent(_outsideParent);
            return obj;
        }

        public T Take(Transform customParent)
        {
            var obj = _pool.Take();
            
            obj.transform.SetParent(customParent);
            return obj;
        }

        public void Release(T obj)
        {
            obj.transform.SetParent(_insideParent);
            _pool.Release(obj);
        }

        private void OnValidate()
        {
            ValidateParents();
        }

        private void ValidateParents()
        {
            if (_insideParent == null)
            {
                var inside = transform.Find("inside");
                if (inside != null)
                {
                    _insideParent = inside.transform;
                }

                if (_insideParent == null)
                {
                    _insideParent = new GameObject("inside").transform;
                    _insideParent.SetParent(transform);
                    _insideParent.gameObject.SetActive(false);
                }
            }

            if (_outsideParent == null)
            {
                var outside = transform.Find("outside");
                if (outside != null)
                {
                    _outsideParent = outside.transform;
                }

                if (_outsideParent == null)
                {
                    _outsideParent = new GameObject("outside").transform;
                    _outsideParent.SetParent(transform);
                }
            }
        }
    }
}