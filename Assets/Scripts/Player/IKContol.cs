using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boxing
{
    [RequireComponent(typeof(Animator))]
    public class IKContol : MonoBehaviour
    {
        [SerializeField] private Transform _spine;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

    }
}
