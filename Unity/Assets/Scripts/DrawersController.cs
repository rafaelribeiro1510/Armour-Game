using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrawersController : MonoBehaviour
{
    List<SingleDrawerBehaviour> Drawers = new List<SingleDrawerBehaviour>();
    

    private void Awake() {
        foreach(Transform child in transform) Drawers.Add(child.GetComponent<SingleDrawerBehaviour>());
    }

    private void Start() {
        GenerateRandomPairsOfDrawers();
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

            print("Made pair from " + id1 + " and " + id2);
        }


    }
}
