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

        public void InsertBodyInputInfo(BodyPartType bodyPartType, BodyInputInfo bodyInputInfo, bool skipSave)
        {
            _bodyInputInfo.Add(bodyPartType, bodyInputInfo);
            _bodyPartBehaviours[bodyPartType].SetBodyInputInfo(bodyInputInfo);
            
            if (!skipSave && bodyPartState == BodyPartState.Physical) // Only one of the bodies executes saving
                SaveState();
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

        public void SaveState()
        {
            // Make sure saves folder exists
            string saveFolder = Application.persistentDataPath + "/saves";
            Directory.CreateDirectory(saveFolder);

            var serialized = JsonConvert.SerializeObject(_bodyInputInfo);
            string destination = saveFolder + "/save_" /* + saveSlot + "_" */ + ".dat";
            File.WriteAllText(destination, serialized);
            print("Saved " + bodyPartState + serialized);
        }

        public Dictionary<BodyPartType, BodyInputInfo> LoadState()
        {
            string destination = Application.persistentDataPath + "/saves/save_" /* + saveSlot + "_" */ + ".dat";
            try
            {
                var fileStream = File.OpenRead(destination);
                string saveFile;
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    saveFile = reader.ReadToEnd();
                }

                _bodyInputInfo.Clear();
                var deserialized = JsonConvert.DeserializeObject<Dictionary<BodyPartType, BodyInputInfo>>(saveFile);
                if (deserialized == null) return null;
                print("Loaded " + bodyPartState + deserialized);
                
                _bodyInputInfo.Clear();

                return deserialized;
            }
            catch (FileNotFoundException e)
            {
                print(e);
            }

            return null;
        }
    }
}
