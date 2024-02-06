using System;
using UnityEngine;

namespace Boxing
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventsReceiver : MonoBehaviour
    {
        public event Action<int> OnKick;
        public event Action OnJumpHit;


        private void Kick(int value)
        {
            OnKick?.Invoke(value);
        }

        private void JumpHit()
        {
            OnJumpHit?.Invoke();
        }
    }
}
