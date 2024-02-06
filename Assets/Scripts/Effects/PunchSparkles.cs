using UnityEngine;

namespace Boxing
{
    public class PunchSparkles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        private Puncher[] _punchers;

        private void Awake()
        {
            _punchers = GetComponentsInChildren<Puncher>();
            foreach (var item in _punchers)
            {
                item.OnHit += EmitSparkles;
            }
        }

        private void EmitSparkles(HitInfo hitInfo)
        {
            _particleSystem.transform.position = hitInfo.position;
            _particleSystem.Emit(1);
        }
    }
}