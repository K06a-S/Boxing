using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Boxing
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth = 100;
        private int _currentHealth;
        [SerializeField] private GameObject _hpBarPrefab;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;

        public event Action<DamageInfo> OnDamaged;
        public event Action<DamageInfo> OnDefeat;
        public event Action<Vector3> OnKnockOut;
        //public event Action OnRestore;

        [Inject]
        private void Init(Canvas canvas)
        {
            _currentHealth = _maxHealth;
            var hpBar = Instantiate(_hpBarPrefab, canvas.transform).GetComponent<HealthBar>();
            hpBar.Init(this);
            //hpBar.GetComponent<UIWorldFollower>().SetTarget(transform);
        }

        public void TakeDamage(HitInfo hitInfo)
        {
            if (_currentHealth == 0) return;
            var previous = _currentHealth;
            _currentHealth = Mathf.Max(0, _currentHealth - hitInfo.damage);
            var dmgInfo = new DamageInfo(_currentHealth, _maxHealth, hitInfo.damage, hitInfo);
            OnDamaged?.Invoke(dmgInfo);
            if (_currentHealth == 0)
            {
                //OnKnockOut?.Invoke(hitInfo.GetDirection());
                OnDefeat?.Invoke(dmgInfo);
            }
            else
            {
                if (hitInfo.doKnockOut)
                {
                    OnKnockOut?.Invoke(hitInfo.GetDirection());
                    //StartCoroutine(RestoreRoutine(_knockOutDuration));
                }
            }
        }

        //private IEnumerator RestoreRoutine(float duretion)
        //{
        //    yield return new WaitForSeconds(duretion);
        //    OnRestore?.Invoke();
        //}

    }

    public struct DamageInfo
    {
        public int currentHealth;
        public int maxHealth;
        public int dealed;
        public HitInfo hitInfo;

        public DamageInfo(int current, int max, int dealed, HitInfo hitInfo)
        {
            currentHealth = current;
            maxHealth = max;
            this.dealed = dealed;
            this.hitInfo = hitInfo;
        }

        public float GetRatio()
        {
            return currentHealth / (float)maxHealth;
        }
    }

    public interface IDamageable
    {
        int MaxHealth { get; }
        int CurrentHealth { get; }

        event Action<DamageInfo> OnDamaged;
        event Action<DamageInfo> OnDefeat;
        void TakeDamage(HitInfo hitInfo);
    }
}
