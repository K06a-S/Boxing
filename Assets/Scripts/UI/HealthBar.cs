using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Boxing
{
    public class HealthBar : MonoBehaviour
    {
        private Damageable _target;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _backImage;
        [SerializeField] private float _animationDuration = 0.4f;
        private TMP_Text _text;

        private void OnDestroy()
        {
            if (_target != null)
            _target.OnDamaged -= SetValue;
        }

        public void Init(Damageable target)
        {
            _target = target;
            _target.OnDamaged += SetValue;

            _text = GetComponentInChildren<TMP_Text>();
            _text.text = _target.CurrentHealth.ToString();
            float ratio = _target.CurrentHealth / (float)_target.MaxHealth;
            _fillImage.fillAmount = ratio;
            _backImage.fillAmount = ratio;

            GetComponent<UIWorldFollower>().SetTarget(_target.GetComponentInChildren<Renderer>());
        }

        private void SetValue(DamageInfo damageInfo)
        {
            _text.text = damageInfo.currentHealth.ToString();
            float ratio = damageInfo.GetRatio();
            _fillImage.fillAmount = ratio;
            _backImage.DOFillAmount(ratio, _animationDuration);
        }
    }
}
