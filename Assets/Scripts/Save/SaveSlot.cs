using System;
using System.Collections.Generic;
using System.IO;
using Body;
using Body.BodyType;
using Controllers;
using DG.Tweening;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Save
{
    public class SaveSlot : MonoBehaviour
    {
        private Camera _camera;
        private Collider2D _collider;
        private GameSectionController _gameSectionController;

        [SerializeField] private FinishedBodiesController controller;

        public Dictionary<BodyPartType, BodyInputInfo> SaveInfo;
        public int index;

        private static bool _active = true;
        private void Awake()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider2D>();

            index = transform.GetSiblingIndex() + 1;

            SaveInfo = StateFromFile();
        }

        private void Start()
        {
            _gameSectionController = GameSectionController.Instance;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Ended && _collider == Physics2D.OverlapPoint(touchPos) && _active)
                {
                    controller.saveSlot = index;
                    if (SaveInfo is null)
                    {
                        _gameSectionController.MoveToFirstSection();
                    }
                    else
                    {
                        controller.LoadGame(SaveInfo, delegate { _gameSectionController.MoveToFirstSection(); });
                    }
                    _active = false;
                }
            }
        }

        private Dictionary<BodyPartType, BodyInputInfo> StateFromFile()
        {
            string destination = Application.persistentDataPath + "/saves/save_" + index + ".dat";
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
                return null;
            }
        }
    }
}
