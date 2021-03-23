using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    static GameController _instance;
    public static GameController Instance { get { return _instance; } }
    
    List<SingleDrawerBehaviour> Drawers = new List<SingleDrawerBehaviour>();
    List<SingleDrawerBehaviour> ActiveDrawers = new List<SingleDrawerBehaviour>(2);

    [SerializeField] Object BodyPartPrefab;
    
    private void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            foreach(Transform child in transform) Drawers.Add(child.GetComponent<SingleDrawerBehaviour>());
        }
    }

    private void Start() {
        GenerateRandomPairsOfDrawers();
        GenerateBodyParts();
    }

    void GenerateRandomPairsOfDrawers(){
        List<int> freeIndexes = Enumerable.Range(0, Drawers.Count).ToList<int>();

        while(freeIndexes.Count != 0){
            int id1 = Random.Range(0, freeIndexes.Count);
            SingleDrawerBehaviour drawer1 = Drawers[freeIndexes[id1]]; freeIndexes.RemoveAt(id1);
            int id2 = Random.Range(0, freeIndexes.Count);
            SingleDrawerBehaviour drawer2 = Drawers[freeIndexes[id2]]; freeIndexes.RemoveAt(id2);

            drawer1.pair = drawer2;
            drawer2.pair = drawer1;
        }
    }

    private void GenerateBodyParts()
    {
        List<SingleDrawerBehaviour> fullDrawers = new List<SingleDrawerBehaviour>();

        foreach (BodyPartState bodyState in Enum.GetValues(typeof(BodyPartState)))
        {
            foreach (BodyPartType bodyType in Enum.GetValues(typeof(BodyPartType)))
            {
                SingleDrawerBehaviour randomDrawer = Drawers[0];
                while(fullDrawers.Contains(randomDrawer)) randomDrawer = Drawers[Random.Range(0, Drawers.Count)];
                fullDrawers.Add(randomDrawer);
                
                GameObject newBodyPart = Instantiate(BodyPartPrefab, randomDrawer._transform) as GameObject;
                BodyPartBehaviour newBodyPartBehaviour = newBodyPart.GetComponent<BodyPartBehaviour>();

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
