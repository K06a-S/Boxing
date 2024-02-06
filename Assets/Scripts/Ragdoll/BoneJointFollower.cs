using UnityEngine;

namespace Boxing
{
    public class BoneJointFollower : DisabledOnKnockOut
    {
        [SerializeField] private Transform _target;

        private void FixedUpdate()
        {
            transform.localRotation = _target.localRotation;
        }
    }

    public abstract class DisabledOnKnockOut : MonoBehaviour
    {
        protected virtual void Awake()
        {
            var dmgbl = GetComponentInParent<Damageable>();
            dmgbl.GetComponentInChildren<Ragdoll>().OnKOStateActive += Disable;
        }
        private void Disable(bool value)
        {
            enabled = !value;
        }
    }
}