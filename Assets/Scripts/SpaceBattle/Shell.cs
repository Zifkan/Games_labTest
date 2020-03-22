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
        private Vector3 _direction;
        
        public void Init(ObjectPool<Shell> shellPool, float damage, Vector3 direction)
        {
            _damage = damage;
            _shellPool = shellPool;
            _direction = direction;
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
            transform.position += _direction * Time.deltaTime * _movementSpeed;
        }
    }
}