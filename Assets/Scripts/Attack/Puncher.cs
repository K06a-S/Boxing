using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boxing
{
    public class Puncher : MonoBehaviour, IHitter
    {
        [SerializeField] private int damage = 10;
        [SerializeField] private Transform[] _fists;
        [SerializeField] private SphereHitChecker _hitChecker;
        private IAttackTrigger _trigger;
        private Animator _animator;
        private int _attacBoolID;
        private Faction _faction;
        private AnimatorEventsReceiver _eventsReceiver;

        public event Action<HitInfo> OnHit;

        private void Awake()
        {
            _faction = GetComponentInParent<Faction>();
            _animator = _faction.GetComponentInChildren<Animator>();
            _attacBoolID = Animator.StringToHash("IsAttacking");
            _trigger = GetComponentInChildren<IAttackTrigger>();
            _eventsReceiver = _faction.GetComponentInChildren<AnimatorEventsReceiver>();
            _eventsReceiver.OnKick += DoKick;
            if (_trigger != null)
            {
                _trigger.OnAttackTriggered += EnablePunch;
            }
            else
            {
                Debug.LogWarning("IAttackTrigger not found.");
            }
        }

        private void OnDestroy()
        {
            if (_trigger != null) _trigger.OnAttackTriggered -= EnablePunch;
            if (_eventsReceiver != null) _eventsReceiver.OnKick -= DoKick;
        }

        private void EnablePunch(bool enable)
        {
            _animator.SetBool(_attacBoolID, enable);
        }

        private void DoKick(int value)
        {
            var fist = _fists[value];
            GameObject go = _hitChecker.Check(fist.position, _faction.AttackMask);
            if (go)
            {
                OnHit?.Invoke(new HitInfo(fist.position, damage, go, _faction.gameObject, false));
            }
            //print($"{_faction.gameObject.name} kick {value}. result {go != null}");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            foreach (var fist in _fists)
            {
                _hitChecker.DrawGizmos(fist.position);
            }
        }
    }

    [Serializable]
    public class SphereHitChecker
    {
        [SerializeField] private float _hitRadius = 0.2f;
        private Collider[] _hitColliders = new Collider[1];

        public GameObject Check(Vector3 position, LayerMask mask)
        {
            if (Physics.OverlapSphereNonAlloc(position, _hitRadius, _hitColliders, mask) > 0)
            {
                return _hitColliders[0].gameObject;
            }
            return null;
        }

        public void DrawGizmos(Vector3 position)
        {
            Gizmos.DrawWireSphere(position, _hitRadius);
        }
    }
}
