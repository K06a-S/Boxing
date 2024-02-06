using System.Collections;
using UnityEngine;

namespace Boxing
{
    public class RagdollRootMover : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        private Transform _parent;
        public bool isMoving;

        private void Awake()
        {
            var ragdoll = GetComponent<Ragdoll>();
            ragdoll.OnKOStateActive += Move;
            _parent = transform.parent;
        }

        private void Move(bool value)
        {
            isMoving = value;
            if (value)
            {
                StopAllCoroutines();
                StartCoroutine(MoveRoutine());
            }
        }

        private IEnumerator MoveRoutine()
        {
            transform.SetParent(null);
            while (isMoving)
            {
                Vector3 pos = transform.position;
                pos.y = _root.position.y;
                _root.position = pos;

                yield return new WaitForFixedUpdate();
            }
            transform.SetParent(_parent);
        }
    }
}