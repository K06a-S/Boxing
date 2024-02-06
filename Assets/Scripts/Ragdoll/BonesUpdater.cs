using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Boxing
{
    public class BonesUpdater : DisabledOnKnockOut
    {
        [SerializeField] private Transform _sourceRoot;
        private List<BonesPair> bonesPairs = new();

        protected override void Awake()
        {
            base.Awake();
            var receiverBones = transform.GetComponentsInChildren<Transform>().Where(tr => tr.GetComponent<BoneJointFollower>() == null);
            var sourceBones = _sourceRoot.GetComponentsInChildren<Transform>();
            foreach (var rBone in receiverBones)
            {
                foreach (var sBone in sourceBones)
                {
                    if (rBone.name == sBone.name)
                    {
                        bonesPairs.Add(new(sBone, rBone));
                        break;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            foreach(var bPair in bonesPairs)
            {
                bPair.Update();
            }
        }

        [Serializable]
        public class BonesPair
        {
            public readonly Transform source;
            public readonly Transform receiver;

            public BonesPair(Transform source, Transform receiver)
            {
                this.source = source;
                this.receiver = receiver;
            }

            public void Update()
            {
                receiver.localPosition = source.localPosition;
                receiver.localRotation = source.localRotation;
            }
        }
    }
}