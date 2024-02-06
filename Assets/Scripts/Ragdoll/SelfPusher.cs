using UnityEngine;

namespace Boxing
{
    public class SelfPusher : MonoBehaviour
    {
        [SerializeField] private float _force = 40;
        private Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = false;
            GetComponentInParent<Damageable>().OnDamaged += Push;
        }

        private void Push(DamageInfo damageInfo)
        {
            _rb.AddForce(_force * damageInfo.hitInfo.GetDirection(), ForceMode.Impulse);
        }
    }
}