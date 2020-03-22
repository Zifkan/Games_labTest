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

        public void Init(float damage)
        {
            _damage = damage;
        }

        private void OnCollisionEnter(Collision other)
        {
            var ship = other.gameObject.GetComponent<BaseSpaceShip>();
            
            if (ship!=null && other.gameObject != ship.gameObject)
            {
                ship.GetDamage(_damage);
            }
        }

        public void PrepareToUse()
        {
            
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