using UnityEngine;

namespace Boxing
{
    public class EnemyStateMachine : MonoBehaviour
    {
        private EnemyState _currentState;

        private Defeated _defeated;
        public Boxing Boxing { get; private set; }
        public Ability Ability { get; private set; }
        public Rest Rest { get; private set; }

        private void Awake()
        {
            _defeated = new Defeated(this);
            Boxing = new Boxing(this);
            Ability = new Ability(this);
            Rest = new Rest(this);

            GetComponent<Damageable>().OnDefeat += SetDefeatedState;
        }

        private void SetDefeatedState(DamageInfo damageInfo)
        {
            SetState(_defeated);
        }

        private void Start()
        {
            SetState(Boxing);
        }

        private void Update()
        {
            _currentState.StateUpdate();
        }

        public void SetState(EnemyState state)
        {
            _currentState?.ExitState();
            _currentState = state;
            _currentState.EnterState();
        }
    }

    //[Serializable]
    //public class Transition
    //{
    //    [SerializeField] private int _transitionTargetIndex = 0;
    //    [SerializeReference] private Condition _condition;

    //    public int TransitionTargetIndex => _transitionTargetIndex;

    //    public bool IsAvailable()
    //    {
    //        return true;
    //    }
    //}

    //[Serializable]
    //public class Condition
    //{
        
    //}

    public abstract class EnemyState
    {
        protected readonly EnemyStateMachine _stateMachine;

        public EnemyState(EnemyStateMachine machine)
        {
            _stateMachine = machine;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
        public virtual void StateUpdate() { }
    }

    public class Defeated : EnemyState
    {
        public Defeated(EnemyStateMachine machine) : base(machine) { }

        public override void EnterState()
        {
            _stateMachine.Boxing.ExitState();
            _stateMachine.enabled = false;
        }
    }

    public class Boxing : EnemyState
    {
        private readonly PlayerRotator _rotator;
        private readonly IAttackTrigger _attackTrigger;

        public Boxing(EnemyStateMachine machine) : base(machine)
        {
            _rotator = machine.GetComponent<PlayerRotator>();
            _attackTrigger = machine.GetComponentInChildren<IAttackTrigger>();
        }

        public override void EnterState()
        {
            _rotator.enabled = true;
            _attackTrigger.Enable();
        }

        public override void ExitState()
        {
            _rotator.enabled = false;
            _attackTrigger.Disable();
        }

        public override void StateUpdate()
        {
            if (!_attackTrigger.IsTriggered && _stateMachine.Ability.IsReady)
            {
                _stateMachine.SetState(_stateMachine.Ability);
            }
        }
    }

    public class Ability : EnemyState
    {
        private EnemyAbilityManager _abilityManager;
        private float _readyTime;
        private Animator _animator;

        public bool IsReady => _readyTime <= Time.time;

        public Ability(EnemyStateMachine machine) : base(machine)
        {
            _abilityManager = machine.GetComponentInChildren<EnemyAbilityManager>();
            _animator = machine.GetComponentInChildren<Animator>();
            RefreshTimer();
        }

        public override void EnterState()
        {
            _abilityManager.Execute();
            _animator.SetBool("doRest", true);
        }

        public override void ExitState()
        {
            if (_abilityManager.IsInProgress)
            {
                _abilityManager.Abort();
            }
        }

        public void RefreshTimer()
        {
            _readyTime = Time.time + Random.Range(_abilityManager.AbilityInterval.x, _abilityManager.AbilityInterval.y);
        }

        public override void StateUpdate()
        {
            if (!_abilityManager.IsInProgress)
            {
                _stateMachine.SetState(_stateMachine.Rest);
            }
        }

    }

    public class Rest : EnemyState
    {
        private Animator _animator;
        private float _restDuration = 3;
        private float _endTime;

        public Rest(EnemyStateMachine machine) : base(machine)
        {
            _animator = machine.GetComponentInChildren<Animator>();
        }

        public override void EnterState()
        {
            _endTime = Time.time + _restDuration;
        }

        public override void ExitState()
        {
            _animator.SetBool("doRest", false);
            _stateMachine.Ability.RefreshTimer();
        }

        public override void StateUpdate()
        {
            if (_endTime <= Time.time)
            {
                _stateMachine.SetState(_stateMachine.Boxing);
            }
        }
    }
}