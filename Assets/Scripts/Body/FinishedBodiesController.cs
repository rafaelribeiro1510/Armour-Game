using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Body.BodyType;
using UnityEngine;

namespace Body
{
    public class FinishedBodiesController : MonoBehaviour
    {
        private FinishedBody _physicalController;
        private FinishedBody _diseaseController;
        private readonly Dictionary<BodyPartType, Dictionary<BodyPartState, BodyPartBehaviour>> _bodyParts = new Dictionary<BodyPartType, Dictionary<BodyPartState, BodyPartBehaviour>>();
        
        void Awake()
        {
            var controllers = GetComponentsInChildren<FinishedBody>();
            _physicalController = controllers[0];
            _diseaseController = controllers[1];
        }

        private void Start()
        {
            // Ran after Awake to make sure body parts have been loaded
            var bodyPartBehaviours = GameObject.FindGameObjectsWithTag("Bodypart")
                .Select(obj => obj.GetComponentInParent<BodyPartBehaviour>());
            foreach (var bodyPartBehaviour in bodyPartBehaviours)
            {
                if (!_bodyParts.ContainsKey(bodyPartBehaviour.BodyType))
                    _bodyParts[bodyPartBehaviour.BodyType] = new Dictionary<BodyPartState, BodyPartBehaviour>()
                    {
                        {bodyPartBehaviour.BodyPartState, bodyPartBehaviour}
                    };
                else 
                    _bodyParts[bodyPartBehaviour.BodyType][bodyPartBehaviour.BodyPartState] = bodyPartBehaviour;
            }
        }

        public bool IsFinished => _physicalController.IsFinished && _diseaseController.IsFinished;
        
        public void InsertBodyInputInfo(BodyPartType bodyPartType, BodyInputInfo bodyInputInfo)
        {
            _physicalController.InsertBodyInputInfo(bodyPartType, bodyInputInfo); 
            _diseaseController.InsertBodyInputInfo(bodyPartType, bodyInputInfo);
        }
        
        [ContextMenu("Save Game")]
        public void SaveState()
        {
            _physicalController.SaveState();
        }

        [ContextMenu("Load Game")]
        public void LoadState()
        {
            var deserialized = _physicalController.LoadState();
            StartCoroutine(PlaceParts_CO(deserialized));
        }

        private IEnumerator PlaceParts_CO(Dictionary<BodyPartType, BodyInputInfo> bodyInputInfos)
        {
            foreach (KeyValuePair<BodyPartType, BodyInputInfo> pair in bodyInputInfos)
            {
                _bodyParts[pair.Key][BodyPartState.Physical].PlaceCorrectly(pair.Value);
                yield return new WaitForSeconds(0.1f);
                _bodyParts[pair.Key][BodyPartState.Disease].PlaceCorrectly(pair.Value);
            }
        }
    }
}
