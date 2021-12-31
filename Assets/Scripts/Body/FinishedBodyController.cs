using System;
using System.Collections.Generic;
using System.IO;
using Body.BodyType;
using DG.Tweening;
using Target;
using UI;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
using Utils;

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
