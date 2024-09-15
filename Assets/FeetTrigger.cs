using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetTrigger : MonoBehaviour
{

    [SerializeField] PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer 3 = level
        {
            playerMovement.isGrounded = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer 3 = level
        {
            playerMovement.isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer 3 = level
        {
            playerMovement.isGrounded = false;
        }
    }
}
