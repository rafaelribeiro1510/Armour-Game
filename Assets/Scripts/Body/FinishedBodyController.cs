using Body.BodyType;
using UnityEngine;

namespace Body
{
    public class FinishedBodyController : MonoBehaviour
    {
        private FinishedBody _physicalController;
        private FinishedBody _diseaseController;
        void Awake()
        {
            var controllers = GetComponentsInChildren<FinishedBody>();
            _physicalController = controllers[0];
            _diseaseController = controllers[1];
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
            _diseaseController.SaveState();
        }

        [ContextMenu("Load Game")]
        public void LoadState()
        {
            _physicalController.LoadState();
            _diseaseController.LoadState();
        }
    }
}
