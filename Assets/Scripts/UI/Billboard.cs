using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boxing
{
    [ExecuteAlways]
    public class Billboard : MonoBehaviour
    {
        private Camera m_Camera;

        private void Awake()
        {
            m_Camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (m_Camera == null) return;
            //var rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _step * Time.deltaTime);
            //transform.LookAt(m_Camera.transform.position);
            transform.rotation = m_Camera.transform.rotation;
        }
    }
}
