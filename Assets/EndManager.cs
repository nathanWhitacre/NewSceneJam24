using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    private GameObject cable;
    private GameObject chain;

    // Start is called before the first frame update
    void Start()
    {
        cable = GameObject.FindGameObjectWithTag("Player");
        chain = GameObject.FindGameObjectWithTag("Chain");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beginEnd() {
        chain.GetComponent<ChainMovement>().enabled = false;
        cable.GetComponent<PlayerMovement>().enabled = false;

        
    }
}
