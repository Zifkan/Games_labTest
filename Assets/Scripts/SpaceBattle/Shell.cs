using SpaceBattle.SpaceShips;
using SpaceBattle.Utils;
using UnityEngine;

namespace SpaceBattle
{
    public class Shell : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private float _movementSpeed;
        
        private float _damage;
        private ObjectPool<Shell> _shellPool;
        public void Init(ObjectPool<Shell> shellPool,float damage)
        {
            _damage = damage;
            _shellPool = shellPool;
        }

        private void OnCollisionEnter(Collision other)
        {
            var ship = other.gameObject.GetComponent<BaseSpaceShip>();
            
            if (ship!=null && gameObject != ship.gameObject)
            {
                ship.GetDamage(_damage);
                _shellPool.ReturnToPool(this);
            }
        }

        public void PrepareToUse()
        {
            gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
            
        }

        private void Update()
        {
            transform.position += Vector3.forward * Time.deltaTime * _movementSpeed;
        }
    }
}