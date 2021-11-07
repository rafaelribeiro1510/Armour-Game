using System;
using System.Collections.Generic;
using Body.BodyType;
using DG.Tweening;
using Target;
using UI;
using UnityEngine;
using Utils;

namespace Body
{
    public class FinishedBody : MonoBehaviour
    {
        private readonly Dictionary<BodyPartType, BodyInputInfo> _bodyInputInfo = new Dictionary<BodyPartType, BodyInputInfo>();
        private readonly Dictionary<BodyPartType, FinishedBodyPartBehaviour> _bodyPartBehaviours = new Dictionary<BodyPartType, FinishedBodyPartBehaviour>();

        private OverlapBodiesButton _overlapBodiesButton;
        private bool _localOverlapFlag = false;
        private const float DisplacementDuration = 0.5f;

        public BodyPartState bodyPartState;

        void Awake()
        {
            var bodyPartBehaviours = GetComponentsInChildren<FinishedBodyPartBehaviour>();
            foreach (var bodyPartBehaviour in bodyPartBehaviours)
            {
                _bodyPartBehaviours.Add(bodyPartBehaviour.BodyType, bodyPartBehaviour);
            }
            
            _overlapBodiesButton = OverlapBodiesButton.Instance;
        }

        public bool IsFinished => _bodyInputInfo.Count == 1;

        public void InsertBodyInputInfo(BodyPartType bodyPartType, BodyInputInfo bodyInputInfo)
        {
            _bodyInputInfo.Add(bodyPartType, bodyInputInfo);
            _bodyPartBehaviours[bodyPartType].SetBodyInputInfo(bodyInputInfo);
        }
        
        private void FadeAlphaOfChildren(float alpha) {
            SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer child in children) {
                child.DOFade(alpha, DisplacementDuration);
            }
        }

        private void Update()
        {
            // Handle OverlapBodiesButton changes (only run once when button is clicked)
            if (_localOverlapFlag == _overlapBodiesButton.isOverlapping) return;

            var displacementX = 0;
            if (bodyPartState == BodyPartState.Physical)
            {
                displacementX = _overlapBodiesButton.isOverlapping ? 5 : -5;
            }
            else if (bodyPartState == BodyPartState.Disease)
            {
                displacementX = _overlapBodiesButton.isOverlapping ? -5 : 5;
            }

            // Translate body
            transform.DOMoveX(displacementX, DisplacementDuration).SetRelative();
                
            // If this is the Disease body, also fade opacity
            FadeAlphaOfChildren(_overlapBodiesButton.isOverlapping ? 0.5f : 1);
            
            _localOverlapFlag = _overlapBodiesButton.isOverlapping;
        }
    }
}
