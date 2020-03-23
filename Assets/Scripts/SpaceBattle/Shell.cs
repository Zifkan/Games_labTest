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
        private Vector3 _startPos;

        public void Init(ObjectPool<Shell> shellPool, float damage, Vector3 direction)
        {
            _damage = damage;
            _shellPool = shellPool;
            _direction = direction;

            _startPos = transform.position;
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

            if (Physics.Raycast(new Ray(transform.position,transform.forward),out RaycastHit hitInfo,1f))
            {
                var ship = hitInfo.transform.GetComponent<BaseSpaceShip>();
            
                if (ship!=null && gameObject != ship.gameObject)
                {
                    ship.GetDamage(_damage);
                    _shellPool.ReturnToPool(this);
                }
            }

            if (Vector3.Distance(_startPos,transform.position) > 100)
            {
                _shellPool.ReturnToPool(this);
            }
        }
        
        
    }
}