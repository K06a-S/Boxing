using UnityEngine;

namespace Boxing
{
    public class UIWorldFollower : MonoBehaviour
    {
        [SerializeField] private Renderer _target;
        [SerializeField] bool _isPlayer;
        [SerializeField] private float _offsetY;

        private void LateUpdate()
        {
            if (_target != null)
            {
                Vector3 pos = _target.transform.position;
                if ( _isPlayer )
                {
                    pos.y = _target.bounds.min.y + _offsetY;
                }
                else
                {
                    pos.y = _target.bounds.max.y + _offsetY;
                }
                transform.position = pos;
            }
        }

        public void SetTarget(Renderer target)
        {
            _target = target;
        }
    }
}
