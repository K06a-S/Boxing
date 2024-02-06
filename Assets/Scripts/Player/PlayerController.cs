using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boxing.Input;
using UnityEngine.InputSystem;
using System;
using Zenject;

namespace Boxing
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed = 4;
        [SerializeField] private float _moveForce = 50;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private GameInput _input;
        private Vector2 _direction;
        private float _speed;
        private float animationBlendX;
        private float animationBlendY;
        [SerializeField] private float _blendSpeed = 0.1f;
        //private int _layerIndex;
        private IAttackTrigger _attackTrigger;

        [Inject]
        private void Init(GameInput input)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
            _animator = GetComponentInChildren<Animator>();
            //_layerIndex = _animator.GetLayerIndex("UpperBody");
            _input = input;
            _input.Enable();
            _input.Player.Move.performed += MovePerformed;

            _attackTrigger = GetComponentInChildren<IAttackTrigger>();
            var ragdoll = GetComponentInChildren<Ragdoll>();
            ragdoll.OnKOStateActive += Enable;
        }

        private void Enable(bool value)
        {
            if (value)
            {
                _input.Player.Disable();
                _attackTrigger.Disable();
                enabled = false;
            }
            else
            {
                _input.Player.Enable();
                _attackTrigger.Enable();
                enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (_input.Player.Move.IsPressed())
            {
                Move();
            }

            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
            _speed = _rigidbody.velocity.magnitude;

            float speedRatio = _speed / _maxSpeed;
            animationBlendX = Mathf.MoveTowards(animationBlendX, _direction.x * speedRatio, _blendSpeed);
            animationBlendY = Mathf.MoveTowards(animationBlendY, _direction.y * speedRatio, _blendSpeed);
            _animator.SetFloat("Forward", animationBlendY);
            _animator.SetFloat("Right", animationBlendX);

            //float rotationX = _attackTrigger.IsTriggered ?.
            //_animator.transform.localRotation = Quaternion.Euler(0, 90 * animationBlendX, 0);
            //_animator.SetLayerWeight(_layerIndex, speedRatio);
        }

        private void OnDestroy()
        {
            _input.Player.Move.performed -= MovePerformed;
        }

        private void MovePerformed(InputAction.CallbackContext callback)
        {
            _direction = callback.ReadValue<Vector2>();
        }

        private void Move()
        {
            var moveVector = transform.TransformDirection(new Vector3(_direction.x, 0, _direction.y));
            _rigidbody.AddForce(moveVector * _moveForce, ForceMode.Acceleration);
        }
    }
}