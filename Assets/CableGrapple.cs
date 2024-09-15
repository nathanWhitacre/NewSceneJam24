using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableGrapple : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraLook cameraLook;
    [SerializeField] private Rigidbody playerRigidBody;

    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private float pullDuration = 0.25f;

    public bool isGrappling;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(cameraLook.eyePosition, (cameraLook.facingDirection * 4f), Color.magenta);


        //Use Grapple
        if (Input.GetMouseButtonDown(0) && !isGrappling) // Left Click
        {
            StartCoroutine(useGrapple());
        }
    }


    private IEnumerator useGrapple()
    {
        isGrappling = true;
        playerMovement.isRooted = true;
        yield return new WaitForSeconds(0.05f);
        playerMovement.isRooted = false;
        playerMovement.isLocked = true;
        playerMovement.isWeightless = true;
        playerRigidBody.velocity += cameraLook.facingDirection * pullStrength;
        yield return new WaitForSeconds(pullDuration);
        playerMovement.isWeightless = false;
        playerMovement.isLocked = false;
        playerMovement.isFlying = true;
        isGrappling = false;
        /*
        while (!playerMovement.isGrounded)
        {
            yield return new WaitForEndOfFrame();
        }
        playerMovement.isLocked = false;
        */
        //isGrappling = false;
    }
}
