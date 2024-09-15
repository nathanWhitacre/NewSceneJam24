using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarTriggerSphere : MonoBehaviour
{
    private AltarSheepController altarParent;
    // Start is called before the first frame update
    void Start()
    {
        altarParent = this.gameObject.GetComponentInParent<AltarSheepController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Player") {
            if (c.GetComponent<PlayerMovement>().hasSheep == true) {
                c.GetComponent<PlayerMovement>().hasSheep = false;
                //add update to player UI
                altarParent.IncrementSheep();
            }
        }
    }
}
