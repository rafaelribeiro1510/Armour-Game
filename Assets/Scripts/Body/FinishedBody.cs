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

        private SplitBodiesButton _splitBodiesButton;
        private bool _localSplitFlag = false;
        private const float DisplacementDuration = 0.5f;

        public BodyPartState bodyPartState;

        void Awake()
        {
            var bodyPartBehaviours = GetComponentsInChildren<FinishedBodyPartBehaviour>();
            foreach (var bodyPartBehaviour in bodyPartBehaviours)
            {
                _bodyPartBehaviours.Add(bodyPartBehaviour.BodyType, bodyPartBehaviour);
            }
            
            _splitBodiesButton = SplitBodiesButton.Instance;
        }

        public bool IsFinished => _bodyInputInfo.Count == 6;

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
            if (_localSplitFlag == _splitBodiesButton.isOverlapping) return;

            var displacementX = 0;
            if (bodyPartState == BodyPartState.Physical)
            {
                displacementX = _splitBodiesButton.isOverlapping ? -5 : 5;
            }
            else if (bodyPartState == BodyPartState.Disease)
            {
                displacementX = _splitBodiesButton.isOverlapping ? 5 : -5;
            }

            // Translate body
            transform.DOMoveX(displacementX, DisplacementDuration).SetRelative();
                
            // If this is the Disease body, also fade opacity
            FadeAlphaOfChildren(_splitBodiesButton.isOverlapping ? 1 : 0.5f);
            
            _localSplitFlag = _splitBodiesButton.isOverlapping;
        }
    }
}
