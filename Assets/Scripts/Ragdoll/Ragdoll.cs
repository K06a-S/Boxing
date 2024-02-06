using System;
using System.Collections;
using UnityEngine;

namespace Boxing
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private float _knockOutDuration = 2;
        [SerializeField] private float _knockOutForce = 50;
        public Rigidbody[] _rigidbodies;
        private Collider[] _colliders;
        private Rigidbody _mainRB;
        private Collider _mainCollider;
        private Animator _animator;
        [SerializeField] private AnimationBonesHelper _bonesHelper;

        public event Action<bool> OnKOStateActive;

        private void Start()
        {
            var dmgbl = GetComponentInParent<Damageable>();
            _animator = dmgbl.GetComponentInChildren<Animator>();
            _mainCollider = dmgbl.GetComponent<Collider>();
            _mainRB = dmgbl.GetComponent<Rigidbody>();
            dmgbl.OnKnockOut += KnockOutTemp;
            dmgbl.OnDefeat += dmgInfo => KnockOut(dmgInfo.hitInfo.GetDirection());

            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();

            SetMainPhysicComponentsActive(true);
            SetRagdollActive(false);

            _bonesHelper.InitBones(transform.parent);
        }

        private void KnockOut(Vector3 direction)
        {
            BaseKnockOut(true);
            _animator.enabled = false;
            OnKOStateActive?.Invoke(true);
            foreach (Rigidbody rb in _rigidbodies)
            {
                rb.AddForce(direction * _knockOutForce, ForceMode.Impulse);
            }
        }

        private void Restore()
        {
            var parent = transform.parent;
            transform.SetParent(null);

            Vector3 pos = transform.position;
            pos.y = _mainRB.transform.position.y;
            _mainRB.transform.position = pos;

            transform.SetParent(parent);

            BaseKnockOut(false);
            StartCoroutine(_bonesHelper.BonesInterpolationRoutine(FinishRestore));
        }

        private void FinishRestore()
        {
            _animator.enabled = true;
            OnKOStateActive?.Invoke(false);
        }

        private void SetRagdollActive(bool value)
        {
            foreach (Rigidbody rb in _rigidbodies)
            {
                rb.isKinematic = !value;
            }
            foreach (Collider collider in _colliders)
            {
                collider.enabled = value;
            }
        }

        private void SetMainPhysicComponentsActive(bool value)
        {
            _mainCollider.enabled = value;
            if (_mainRB != null) _mainRB.isKinematic = !value;
        }

        private void BaseKnockOut(bool value)
        {
            SetMainPhysicComponentsActive(!value);
            SetRagdollActive(value);
            //_animator.enabled = !value;
        }

        public void KnockOutTemp(Vector3 direction)
        {
            KnockOut(direction);
            StartCoroutine(KnockOutRoutine());
        }

        private IEnumerator KnockOutRoutine()
        {
            yield return new WaitForSeconds(_knockOutDuration);
            Restore();
        }
    }

    [Serializable]
    public class AnimationBonesHelper
    {
        [SerializeField] private AnimationClip _clip;
        [SerializeField] private float _standUpDuration = 0.5f;
        public Transform[] _bones;
        private BoneData[] _bonesDataAtAnimationStart;

        public void InitBones(Transform root)
        {
            _bones = root.GetComponentsInChildren<Transform>();

            var initBones = new BoneData[_bones.Length];
            SaveBonesData(initBones);

            _bonesDataAtAnimationStart = new BoneData[_bones.Length];
            _clip.SampleAnimation(root.parent.gameObject, 0);
            SaveBonesData(_bonesDataAtAnimationStart);

            for (int i = 0; i < _bones.Length; i++)
            {
                Transform bone = _bones[i];
                bone.localPosition = initBones[i].position;
                bone.localRotation = initBones[i].rotation;
            }
        }

        private void SaveBonesData(BoneData[] boneDatas)
        {
            for (int i = 0; i < boneDatas.Length; i++)
            {
                var bone = _bones[i];
                boneDatas[i] = new BoneData(bone.localPosition, bone.localRotation);
            }
        }

        public IEnumerator BonesInterpolationRoutine(Action onEnd = null)
        {
            var bonesFrom = new BoneData[_bones.Length];
            SaveBonesData(bonesFrom);

            float time = 0f;
            while (time < _standUpDuration)
            {
                float t = time / _standUpDuration;
                for (int i = 0; i < _bones.Length; i++)
                {
                    Transform bone = _bones[i];
                    bone.localPosition = Vector3.Lerp(bonesFrom[i].position, _bonesDataAtAnimationStart[i].position, t);
                    bone.localRotation = Quaternion.Lerp(bonesFrom[i].rotation, _bonesDataAtAnimationStart[i].rotation, t);
                }

                time += Time.deltaTime;
                yield return null;
            }
            onEnd?.Invoke();
        }

        struct BoneData
        {
            public Vector3 position;
            public Quaternion rotation;

            public BoneData(Vector3 position, Quaternion rotation)
            {
                this.position = position;
                this.rotation = rotation;
            }
        }
    }

}