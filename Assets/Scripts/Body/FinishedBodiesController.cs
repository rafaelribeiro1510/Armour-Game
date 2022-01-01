using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Body.BodyType;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Body
{
    public class FinishedBodiesController : MonoBehaviour
    {
        private List<FinishedBody> _controllers;
        private readonly Dictionary<BodyPartType, Dictionary<BodyPartState, BodyPartBehaviour>> _bodyParts = new Dictionary<BodyPartType, Dictionary<BodyPartState, BodyPartBehaviour>>();
        
        private readonly Dictionary<BodyPartType, BodyInputInfo> _bodyInputInfo = new Dictionary<BodyPartType, BodyInputInfo>();
        
        void Awake()
        {
            _controllers = GetComponentsInChildren<FinishedBody>().ToList();
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

        public bool IsFinished => _bodyInputInfo.Count == 6;
        
        public void InsertBodyInputInfo(BodyPartType bodyPartType, BodyInputInfo bodyInputInfo, bool skipSave)
        {
            foreach (var controller in _controllers)
            {
                controller.InsertBodyInputInfo(bodyPartType, bodyInputInfo);
            }

            if (!skipSave)
                SaveGame();
        }
        
        [ContextMenu("Save Game")]
        public void SaveGame()
        {
            // Make sure saves folder exists
            string saveFolder = Application.persistentDataPath + "/saves";
            Directory.CreateDirectory(saveFolder);

            var serialized = JsonConvert.SerializeObject(_bodyInputInfo);
            string destination = saveFolder + "/save_" /* + saveSlot + "_" */ + ".dat";
            File.WriteAllText(destination, serialized);
            print("Saved " + serialized);
        }

        [ContextMenu("Load Game")]
        public void LoadGame()
        {
            var deserialized = StateFromFile();
            _bodyInputInfo.Clear();
            StartCoroutine(PlaceParts_CO(deserialized));
        }


        private Dictionary<BodyPartType, BodyInputInfo> StateFromFile()
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

                var deserialized = JsonConvert.DeserializeObject<Dictionary<BodyPartType, BodyInputInfo>>(saveFile);
                if (deserialized == null) return null;
                print("Loaded " + deserialized);

                return deserialized;
            }
            catch (FileNotFoundException e)
            {
                print(e);
            }

            return null;
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
