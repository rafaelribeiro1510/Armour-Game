using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Body;
using Body.BodyType;
using Drawer;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class DrawerController : MonoBehaviour
{
    public static DrawerController Instance { get; private set; }

    List<SingleDrawerBehaviour> Drawers = new List<SingleDrawerBehaviour>();
    List<SingleDrawerBehaviour> ActiveDrawers = new List<SingleDrawerBehaviour>(2);
    
    // State machine
    private GameObject TargetController;

    [SerializeField] Object BodyPartPrefab;
    
    private void Awake()
    {
        SingletonInitialization();
        
        Drawers = GetComponentsInChildren<SingleDrawerBehaviour>().ToList();
    }

    private void SingletonInitialization()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        GenerateRandomPairsOfDrawers();
        GenerateBodyParts();
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
        var fullDrawers = new List<SingleDrawerBehaviour>();

        foreach (BodyPartState bodyState in Enum.GetValues(typeof(BodyPartState)))
        {
            foreach (BodyPartType bodyType in Enum.GetValues(typeof(BodyPartType)))
            {
                var randomDrawer = Drawers[0];
                while(fullDrawers.Contains(randomDrawer)) randomDrawer = Drawers[Random.Range(0, Drawers.Count)];
                fullDrawers.Add(randomDrawer);
                randomDrawer.holdingType = bodyType;
                
                var newBodyPart = Instantiate(BodyPartPrefab, randomDrawer._transform) as GameObject;
                if (newBodyPart is null) continue;
                var newBodyPartBehaviour = newBodyPart.GetComponent<BodyPartBehaviour>();

                newBodyPart.name = bodyType.ToString();
                newBodyPart.tag = bodyType.ToString();
                newBodyPart.transform.rotation = Quaternion.identity;
                newBodyPartBehaviour.SetState(bodyType, bodyState);
            }   
        }
    }

    public void ActivatePair(SingleDrawerBehaviour drawer1, SingleDrawerBehaviour drawer2) {
        if (ActiveDrawers.Contains(drawer1) || ActiveDrawers.Contains(drawer2)) return;
        
        if (ActiveDrawers.Count != 0)
            foreach (var oldDrawer in ActiveDrawers) {
                oldDrawer.Close();
            }
        
        if ((object)drawer1 != null && (object)drawer2 != null) ActiveDrawers = new List<SingleDrawerBehaviour>(){drawer1, drawer2}; 
    }
}
