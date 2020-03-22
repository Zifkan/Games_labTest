using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattle.Utils
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Queue<T> _poolableQueue;

        private readonly int _poolSize;
        private readonly Transform _parent;
        private readonly T _objRef;
        public ObjectPool(T obj, int poolSize,Transform parent)
        {
            _objRef = obj;
            _parent = parent;
            _poolSize = poolSize;
            _poolableQueue = new Queue<T>(poolSize);

            for (int i = 0; i < _poolSize; i++)
            {
                var instance = InstantiateObj();
                _poolableQueue.Enqueue(instance);
            }
        }

        public ObjectPool(T obj, int poolSize)
        {
            _objRef = obj;
            _poolSize = poolSize;
            _poolableQueue = new Queue<T>(poolSize);

            for (int i = 0; i < _poolSize; i++)
            {
                var instance = InstantiateObj();
                _poolableQueue.Enqueue(instance);
            }
        }

        private T InstantiateObj()
        {
            var instance = Object.Instantiate(_objRef);
            
            if (_parent != null)
            {
                instance.transform.SetParent(_parent);
            }

            instance.gameObject.SetActive(false);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            return instance;
        }

        public T Get()
        {
            var obj = _poolableQueue.Count > 0 ? _poolableQueue.Dequeue() : InstantiateObj();
            obj.PrepareToUse();
            return obj;
        }
        
        public void ReturnToPool(T instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(_parent);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            _poolableQueue.Enqueue(instance);
        }
    }
}