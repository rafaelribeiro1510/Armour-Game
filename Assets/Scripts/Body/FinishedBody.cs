using System;
using System.Collections.Generic;
using Body.BodyType;
using DG.Tweening;
using Target;
using UnityEngine;
using Utils;

namespace Body
{
    public class FinishedBody : MonoBehaviour
    {
        private readonly Dictionary<BodyPartType, BodyInputInfo> _bodyInputInfo = new Dictionary<BodyPartType, BodyInputInfo>();
        private readonly Dictionary<BodyPartType, FinishedBodyPartBehaviour> _bodyPartBehaviours = new Dictionary<BodyPartType, FinishedBodyPartBehaviour>();

        void Awake()
        {
            var bodyPartBehaviours = GetComponentsInChildren<FinishedBodyPartBehaviour>();
            foreach (var bodyPartBehaviour in bodyPartBehaviours)
            {
                _bodyPartBehaviours.Add(bodyPartBehaviour.BodyType, bodyPartBehaviour);
            }
        }

        public bool IsFinished => _bodyInputInfo.Count == 1;

        public void InsertBodyInputInfo(BodyPartType bodyPartType, BodyInputInfo bodyInputInfo)
        {
            _bodyInputInfo.Add(bodyPartType, bodyInputInfo);
            _bodyPartBehaviours[bodyPartType].SetBodyInputInfo(bodyInputInfo);
        }

    }
}
