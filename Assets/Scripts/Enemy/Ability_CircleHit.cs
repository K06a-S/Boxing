using UnityEngine;
using DG.Tweening;
using System;

namespace Boxing
{
    public class Ability_CircleHit : MonoBehaviour, IEnemyAbility, IHitter
    {
        [SerializeField] private int _damage = 40;
        [SerializeField] private Transform _redCircle;
        [SerializeField] private float _circleGrowRadius = 5;
        [SerializeField] private float _circleGrowDuration = 2;
        [SerializeField] private float _pauseDuration = 1;
        private Collider[] _castResults = new Collider[1];
        private Faction _faction;
        private Animator _animator;
        private AnimatorEventsReceiver _eventsReceiver;
        private bool _isInProgress;
        private Sequence _sequence;

        public bool IsInProgress => _isInProgress;

        public event Action<HitInfo> OnHit;

        private void Awake()
        {
            _faction = GetComponentInParent<Faction>();
            _animator = _faction.GetComponentInChildren<Animator>();
            _eventsReceiver = _animator.GetComponent<AnimatorEventsReceiver>();
            _eventsReceiver.OnJumpHit += DoJumpHitAttack;
        }

        private void DoJumpHitAttack()
        {
            _redCircle.gameObject.SetActive(false);
            if (Physics.OverlapSphereNonAlloc(_redCircle.position, _circleGrowRadius, _castResults, _faction.AttackMask) > 0)
            {
                var obj = _castResults[0].gameObject;
                OnHit?.Invoke(new HitInfo(obj.transform.position, _damage, obj, _faction.gameObject, true));
                // push
            }
            _isInProgress = false;
        }

        public void Use()
        {
            _isInProgress = true;
            _redCircle.localScale = Vector3.zero;
            _redCircle.gameObject.SetActive(true);

            _sequence = DOTween.Sequence();
            _sequence.Append(_redCircle.DOScale(_circleGrowRadius * 2, _circleGrowDuration));
            _sequence.AppendInterval(_pauseDuration);
            _sequence.onComplete += OnComplete;
        }

        public void Stop()
        {
            _redCircle.gameObject.SetActive(false);
            _sequence?.Kill();
        }

        private void OnComplete()
        {
            _animator.SetTrigger("JumpHit");
        }

        private void OnDrawGizmosSelected()
        {
            if (_redCircle == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_redCircle.position, _circleGrowRadius);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }

    public struct HitInfo
    {
        public Vector3 position;
        public int damage;
        public GameObject receiver;
        public GameObject dealer;
        public bool doKnockOut;

        public HitInfo(Vector3 position, int damage, GameObject receiver, GameObject dealer, bool doKO)
        {
            this.position = position;
            this.damage = damage;
            this.receiver = receiver;
            this.dealer = dealer;
            doKnockOut = doKO;
        }

        public Vector3 GetDirection()
        {
            return (receiver.transform.position - dealer.transform.position).normalized;
        }
    }

    public interface IHitter
    {
        event Action<HitInfo> OnHit;
    }
}