using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boxing
{
    public class Faction : MonoBehaviour
    {
        [SerializeField] private LayerMask _attackMask;

        public LayerMask AttackMask => _attackMask;
    }
}
