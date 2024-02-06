using UnityEngine;

namespace Boxing
{
    public class JointAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _targeBone;
        private ConfigurableJoint _joint;
        private Quaternion _startRotation;

        void Awake()
        {
            _joint = GetComponent<ConfigurableJoint>();
            _startRotation = transform.localRotation;
        }

        void FixedUpdate()
        {
            _joint.targetRotation = Quaternion.Inverse(_targeBone.localRotation) * _startRotation;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
