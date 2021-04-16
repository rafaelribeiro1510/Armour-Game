using System;
using System.Collections.Generic;
using System.Linq;
using Body;
using Body.BodyType;
using PartCompleteMenu;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Drawer
{
    public class DrawerController : MonoBehaviour
    {
        public static DrawerController Instance { get; private set; }
        private void SingletonInitialization()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            } else {
                Instance = this;
            }
        }

        List<DrawerBehaviour> Drawers = new List<DrawerBehaviour>();
        List<DrawerBehaviour> ActiveDrawers = new List<DrawerBehaviour>(2);
    
        private GameObject TargetController;
        private PartCompleteScript PartCompleteMenu;
        private bool _alreadySlidDrawersOffScreen = false;

        [SerializeField] Object BodyPartPrefab;
    
        private void Awake()
        {
            SingletonInitialization();
        
            Drawers = GetComponentsInChildren<DrawerBehaviour>().ToList();
        }

        private void Start() {
            GenerateRandomPairsOfDrawers();
            GenerateBodyParts();
            
            PartCompleteMenu = PartCompleteScript.Instance;
        }

        private void Update()
        {
            if (!_alreadySlidDrawersOffScreen && PartCompleteMenu.open)
            {
                Drawers.ForEach(drawer => drawer.SlideOffScreen());
                _alreadySlidDrawersOffScreen = true;
            }
            else if (_alreadySlidDrawersOffScreen && !PartCompleteMenu.open)
            {
                Drawers.ForEach(drawer => drawer.SlideOnScreen());
                _alreadySlidDrawersOffScreen = false;
            }
        }

        private void GenerateRandomPairsOfDrawers(){
            var freeIndexes = Enumerable.Range(0, Drawers.Count).ToList<int>();

            while(freeIndexes.Count != 0){
                var id1 = Random.Range(0, freeIndexes.Count);
                var drawer1 = Drawers[freeIndexes[id1]]; freeIndexes.RemoveAt(id1);
                var id2 = Random.Range(0, freeIndexes.Count);
                var drawer2 = Drawers[freeIndexes[id2]]; freeIndexes.RemoveAt(id2);

                drawer1.pair = drawer2;
                drawer2.pair = drawer1;
            }
        }

        private void GenerateBodyParts()
        {
            var fullDrawers = new List<DrawerBehaviour>();

            foreach (BodyPartState bodyState in Enum.GetValues(typeof(BodyPartState)))
            {
                foreach (BodyPartType bodyType in Enum.GetValues(typeof(BodyPartType)))
                {
                    var randomDrawer = Drawers[0];
                    while(fullDrawers.Contains(randomDrawer)) randomDrawer = Drawers[Random.Range(0, Drawers.Count)];

                    var newBodyPart = Instantiate(BodyPartPrefab, randomDrawer._transform) as GameObject;
                    if (newBodyPart is null) continue;
                    var newBodyPartBehaviour = newBodyPart.GetComponent<BodyPartBehaviour>();

                    newBodyPart.name = bodyType.ToString();
                    newBodyPart.tag = bodyType.ToString();
                    newBodyPart.transform.rotation = Quaternion.identity;
                    newBodyPart.transform.position = new Vector3(newBodyPart.transform.position.x, newBodyPart.transform.position.y, -1);
                    newBodyPartBehaviour.SetState(bodyType, bodyState);
                    fullDrawers.Add(randomDrawer);
                    
                    randomDrawer.setHoldingPart(newBodyPartBehaviour);
                }   
            }
        }

        public void ActivatePair(DrawerBehaviour drawer1, DrawerBehaviour drawer2) {
            if (ActiveDrawers.Contains(drawer1) || ActiveDrawers.Contains(drawer2)) return;
        
            if (ActiveDrawers.Count != 0)
                foreach (var oldDrawer in ActiveDrawers) {
                    oldDrawer.Close();
                }
        
            if ((object)drawer1 != null && (object)drawer2 != null) ActiveDrawers = new List<DrawerBehaviour>(){drawer1, drawer2}; 
        }
    }
}
