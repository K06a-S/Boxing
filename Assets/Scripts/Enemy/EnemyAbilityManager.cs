using UnityEngine;
using DG.Tweening;

namespace Boxing
{
    public class EnemyAbilityManager : MonoBehaviour
    {
        [SerializeField] private Vector2 _abilityInterval = new Vector2(5, 7);
        private IEnemyAbility[] _abilities;
        private IEnemyAbility _currentAbility;

        public Vector2 AbilityInterval => _abilityInterval;
        public bool IsInProgress => _currentAbility == null ? false : _currentAbility.IsInProgress;

        private void Awake()
        {
            DOTween.defaultAutoKill = true;
            _abilities = GetComponentsInChildren<IEnemyAbility>();
        }

        private IEnemyAbility GetRandomAbility()
        {
            if (_abilities.Length == 0) return null;
            return _abilities[Random.Range(0, _abilities.Length)];
        }

        public void Execute()
        {
            var ability = GetRandomAbility();
            if (ability != null)
            {
                _currentAbility = ability;
                ability.Use();
            }
        }

        public void Abort()
        {
            _currentAbility?.Stop();
        }
    }

    public interface IEnemyAbility
    {
        bool IsInProgress { get; }
        void Use();
        void Stop();
    }
}