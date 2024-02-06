using UnityEngine;

namespace Boxing
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private Transform _lookTarget;
        [SerializeField] private float _rotationSpeed = 100;

        void FixedUpdate()
        {
            Vector3 direction = _lookTarget.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _rotationSpeed * Time.deltaTime);
        }
    }
}