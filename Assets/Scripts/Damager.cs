using UnityEngine;

namespace Boxing
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private float _multiplier = 1;

        private void Awake()
        {
            var hitters = GetComponentsInChildren<IHitter>();
            foreach (var hitter in hitters)
            {
                hitter.OnHit += DoDamage;
            }
        }

        private void DoDamage(HitInfo hitInfo)
        {
            var enemy = hitInfo.receiver.GetComponent<IDamageable>();
            if (enemy != null)
            {
                hitInfo.damage = Mathf.RoundToInt(hitInfo.damage * _multiplier);
                enemy.TakeDamage(hitInfo);
            }
        }
    }
}