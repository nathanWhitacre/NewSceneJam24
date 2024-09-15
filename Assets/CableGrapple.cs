using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableGrapple : MonoBehaviour
{

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraLook cameraLook;
    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private GameObject grappleRope;
    [SerializeField] private AudioSource grappleUseSound;
    [SerializeField] private AudioSource grappleHitSound;

    [SerializeField] private float pullStrength = 10f;
    [SerializeField] private float pullDuration = 0.25f;
    [SerializeField] private float maxGrappleDistance = 30f;
    [SerializeField] public float cooldown = 10f;

    public float currentCooldown;
    public float currentCharges;
    public bool isGrappling;
    private bool isCoolingDown;
    private bool grappleSuccess;
    private Vector3 grapplePoint;
    private int grappleLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        grappleRope.SetActive(false);
        grappleLayerMask = 1 << 3;
        grappleLayerMask = ~grappleLayerMask;
        currentCharges = 3;
        currentCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        //Use Grapple
        if (Input.GetMouseButtonDown(0) && !isGrappling && currentCharges > 0f) // Left Click
        {
            StartCoroutine(useGrapple());
        }

        //Update Grapple Visual
        if (grappleRope.activeSelf && isGrappling)
        {
            grappleRope.transform.LookAt(grapplePoint);
            grappleRope.transform.Rotate(90, 0, 0, Space.Self);
            grappleRope.transform.localScale = new Vector3(grappleRope.transform.localScale.x, Vector3.Distance(cameraLook.eyePosition, grapplePoint), grappleRope.transform.localScale.z);
        }

        if (currentCharges < 3f && !isCoolingDown)
        {
            StartCoroutine(cooldownGrapple());
        }
    }


    private IEnumerator useGrapple()
    {
        isGrappling = true;
        RaycastHit grappleHit;
        grappleSuccess = Physics.Raycast(cameraLook.eyePosition, cameraLook.facingDirection, out grappleHit, maxGrappleDistance);
        grapplePoint = grappleHit.point;
        grappleRope.SetActive(true);
        grappleUseSound.Play();

        if (grappleSuccess) //Raycast Hit Success
        {
            //Debug.Log("hitLayer: " + grappleHit.collider.gameObject.name + "   |   " + grappleHit.collider.gameObject.layer);
            grappleHitSound.Play();
            playerMovement.isRooted = true;
            yield return new WaitForSeconds(0.05f);
            playerMovement.isRooted = false;
            playerMovement.isLocked = true;
            playerMovement.isWeightless = true;
            playerRigidBody.velocity += cameraLook.facingDirection * pullStrength;
            yield return new WaitForSeconds(pullDuration);
            currentCharges--;
            grappleRope.SetActive(false);
            playerMovement.isWeightless = false;
            playerMovement.isLocked = false;
            playerMovement.isFlying = true;
            isGrappling = false;
        }

        else //Raycast Hit Failure
        {
            grapplePoint = cameraLook.eyePosition + (cameraLook.facingDirection * maxGrappleDistance);
            yield return new WaitForSeconds(pullDuration);
            grappleRope.SetActive(false);
            isGrappling = false;
        }
    }


    private  IEnumerator cooldownGrapple()
    {
        isCoolingDown = true;
        currentCooldown = 0f;
        while (currentCooldown < cooldown)
        {
            currentCooldown += cooldown * 0.01f;
            yield return new WaitForSeconds(cooldown * 0.01f);
        }
        currentCharges++;
        isCoolingDown = false;
    }
}
