using System;
using System.Collections.Generic;
using Body.BodyType;
using UnityEngine;

namespace UI
{
    public class EmotionBoxes : MonoBehaviour
    {
        public Dictionary<BodyPartType, BodyInputInfo> BodyInputInfo;
        private List<EmotionBox> _childrenBoxes;

        private void Awake()
        {
            _childrenBoxes = new List<EmotionBox>(GetComponentsInChildren<EmotionBox>());
        }

        public void SetTexts()
        {
            foreach (var childrenBox in _childrenBoxes)
            {
                var partInfo = BodyInputInfo[childrenBox.bodyPartType];
                childrenBox.SetText(childrenBox.bodyPartState == BodyPartState.Physical ? partInfo.EmotionPhysical : partInfo.EmotionDisease);
            }
        }

        public void ToggleVisible()
        {
            foreach (var childrenBox in _childrenBoxes)
            {
                childrenBox.ToggleVisible();
            }
        }
    }
}
