using System.Collections;
using UnityEngine;

namespace Boxing
{
    public class CameraTarget : MonoBehaviour
    {
        public float speed = 1;
        [SerializeField] private Transform _player;
        private Ragdoll _ragdoll;
        private Transform _target;
        private Transform m_transform;
        public bool isFollowing;

        private void Awake()
        {
            _ragdoll = _player.GetComponentInChildren<Ragdoll>();
            _ragdoll.OnKOStateActive += DoFollow;
            _target = _player;
            m_transform = transform;
        }

        private void FixedUpdate()
        {
            Vector3 pos = _target.position;
            pos.y = m_transform.position.y;
            m_transform.position = pos;
            m_transform.rotation = _player.rotation;
        }

        private void DoFollow(bool value)
        {
            if (value)
            {
                _target = _ragdoll.transform;
                //StopAllCoroutines();
                //isFollowing = true;
                //StartCoroutine(FollowRoutine());
            }
            else
            {
                _target = _player;
                //isFollowing = false;
                //Follow();
            }
        }

        private IEnumerator FollowRoutine()
        {
            while (isFollowing)
            {
                Follow();
                yield return new WaitForFixedUpdate();
            }
        }

        private void Follow()
        {
            Vector3 pos = _ragdoll.transform.position;
            pos.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.fixedDeltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}