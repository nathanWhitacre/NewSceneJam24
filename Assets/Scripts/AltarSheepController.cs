using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarSheepController : MonoBehaviour
{
    private int totalSheep = 7;
    private ChainMovement chainScript;
    private int sheepCount;
    [SerializeField] private Transform[] locations;
    [SerializeField] private GameObject sheepPrefab;
    private GameObject[] sheeps;
    // Start is called before the first frame update
    void Start()
    {
        chainScript = GameObject.FindGameObjectWithTag("Chain").GetComponent<ChainMovement>();
        sheepCount = 1;
        chainScript.ChangeSheepCount(sheepCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementSheep() {
        if (sheepCount < totalSheep) {
            int bias;
            if (sheepCount < 4) {
                bias = -1;
            }
            else {
                bias = 1;
            }
            Instantiate(sheepPrefab, locations[sheepCount-1].position, Quaternion.LookRotation(new Vector3(0, 0, 1 * bias), new Vector3(0, 1, 0)));            
            sheepCount += 1;
            chainScript.ChangeSheepCount(sheepCount);
        }
    }

    void UpdateSheepCount() {
        
    }

}
