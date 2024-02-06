using System;
using UnityEngine;

namespace Boxing
{
    public class AttackTrigger : MonoBehaviour, IAttackTrigger
    {
        [SerializeField] private SphereHitChecker _sphereChecker;
        [SerializeField] private Vector3 _offset;
        private Faction _faction;
        //private Collider m_Collider;
        public event Action<bool> OnAttackTriggered;
        private bool _isTriggered;

        public bool IsTriggered => _isTriggered;

        private void Awake()
        {
            //m_Collider = GetComponent<Collider>();
            _faction = GetComponentInParent<Faction>();
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    IsTriggered = true;
        //    OnAttackTriggered?.Invoke(true, other);
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    IsTriggered = false;
        //    OnAttackTriggered?.Invoke(false, other);
        //}

        private void FixedUpdate()
        {
            var go = _sphereChecker.Check(transform.position + _offset, _faction.AttackMask);
            if ((go == null) == _isTriggered)
            {
                _isTriggered = !_isTriggered;
                OnAttackTriggered?.Invoke(_isTriggered);
            }
        }

        public void Disable()
        {
            if (_isTriggered)
            {
                _isTriggered = false;
                OnAttackTriggered?.Invoke(false);
            }
            enabled = false;
            //m_Collider.enabled = enabled;
        }

        public void Enable()
        {
            enabled = true;
            //m_Collider.enabled = enabled;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            _sphereChecker.DrawGizmos(transform.position + _offset);
        }
    }

    public interface IAttackTrigger
    {
        event Action<bool> OnAttackTriggered;
        bool IsTriggered { get; }

        void Disable();
        void Enable();
    }
}