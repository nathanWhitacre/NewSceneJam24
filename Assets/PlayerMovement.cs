using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Build.Content;
using UnityEditor.Search;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private float topSpeed = 10f;
    [SerializeField] private float topFlyingSpeed = 30f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float flyingAcceleration = 1f;
    [SerializeField] private float groundDeceleration = 0.75f;
    [SerializeField] private float airDeceleration = 0.3f;
    [SerializeField] private float flyingDeceleration = 1f;
    [SerializeField] private float fallAcceleration = 0.2f;
    [SerializeField] private float maxFallSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    public Vector3 currentVelocity;
    public float currentHorizontalSpeed;
    public Vector3 currentInputMovement;
    public float maxSpeed;
    public bool isGrounded;
    public bool isMoving;
    public bool isRooted;
    public bool isWeightless;
    public bool isLocked;
    public bool isJumping;
    public bool isFlying;

    // Start is called before the first frame update
    void Start()
    {
        currentInputMovement = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isLocked && !isRooted)
        {
            //transform.position = transform.position + (Vector3.up * 10f);
            //playerRigidBody.velocity += new Vector3(0f, 0f, 100f);
            //playerRigidBody.velocity += new Vector3(0f, 10f, 0f);
            StartCoroutine(jump());
        }

    }

    private void FixedUpdate()
    {

        //Player Flying
        
        if (isFlying && isGrounded)
        {
            isFlying = false;
        }
        

        //Player Rooted
        if (isRooted)
        {
            playerRigidBody.velocity = Vector3.zero;
            currentVelocity = playerRigidBody.velocity;
            currentHorizontalSpeed = findHorizontalSpeed(currentVelocity);
            isMoving = false;
            return;
        }

        //Movement Initials
        Vector3 newVelocity = playerRigidBody.velocity;
        maxSpeed = 0f;
        float newHorizontalSpeed = findHorizontalSpeed(newVelocity);
        bool movingThisUpdate = false;
        Vector3 newInputMovement = (isLocked) ? Vector3.zero : currentInputMovement;
        float currentTopSpeed = (isFlying) ? topFlyingSpeed : topSpeed;
        float currentAcceleration = (isFlying) ? flyingAcceleration : acceleration;

        //Gravity
        if (isGrounded && !isWeightless)
        {
            newVelocity.y = 0f;
        }
        else if (!isWeightless)
        {
            newVelocity.y -= fallAcceleration;
            if (newVelocity.y <= (-1 * maxFallSpeed)) newVelocity.y = -1f * maxFallSpeed;
        }

        //Moving Forward
        if (Input.GetKey(KeyCode.W) && !isLocked)
        {
            //maxSpeed = topSpeed;
            maxSpeed = currentTopSpeed;
            newInputMovement += transform.forward * currentAcceleration;
            movingThisUpdate = true;
        }

        //Moving Left
        if (Input.GetKey(KeyCode.A) && !isLocked)
        {
            //maxSpeed = topSpeed;
            maxSpeed = currentTopSpeed;
            newInputMovement += (transform.right * -1) * currentAcceleration;
            movingThisUpdate = true;
        }

        //Moving Back
        if (Input.GetKey(KeyCode.S) && !isLocked)
        {
            //maxSpeed = topSpeed;
            maxSpeed = currentTopSpeed;
            newInputMovement += (transform.forward * -1) * currentAcceleration;
            movingThisUpdate = true;
        }

        //Moving Right
        if (Input.GetKey(KeyCode.D) && !isLocked)
        {
            //maxSpeed = topSpeed;
            maxSpeed = currentTopSpeed;
            newInputMovement += transform.right * currentAcceleration;
            movingThisUpdate = true;
        }

        if (movingThisUpdate)
        {
            //Clamp Top Movespeed
            float newInputSpeed = findHorizontalSpeed(newInputMovement);
            if (newInputSpeed > currentTopSpeed)
            {
                newInputMovement.x = newInputMovement.x * (currentTopSpeed / newInputSpeed);
                newInputMovement.z = newInputMovement.z * (currentTopSpeed / newInputSpeed);
            }
            //if (newHorizontalSpeed <= maxSpeed && ((newHorizontalSpeed >= 0f && newInputSpeed >= 0f) || (newHorizontalSpeed <= 0f && newInputSpeed <= 0f))) newVelocity.x = newInputMovement.x;
            //if (newHorizontalSpeed <= maxSpeed && ((newHorizontalSpeed >= 0f && newInputSpeed >= 0f) || (newHorizontalSpeed <= 0f && newInputSpeed <= 0f))) newVelocity.z = newInputMovement.z;
            //if (newHorizontalSpeed <= maxSpeed && Mathf.Abs(newVelocity.x - newInputMovement.x) < 2f) newVelocity.x = newInputMovement.x;
            //if (newHorizontalSpeed <= maxSpeed && Mathf.Abs(newVelocity.z - newInputMovement.z) < 2f) newVelocity.z = newInputMovement.z;
            if (newHorizontalSpeed <= maxSpeed) newVelocity.x = newInputMovement.x;
            if (newHorizontalSpeed <= maxSpeed) newVelocity.z = newInputMovement.z;
            //newVelocity.x = (newHorizontalSpeed <= maxSpeed) ? newInputMovement.x : (newVelocity.x * (newInputMovement.normalized.x));
            //newVelocity.z = (newHorizontalSpeed <= maxSpeed) ? newInputMovement.z : (newVelocity.z * (newInputMovement.normalized.z));
            //newVelocity.x = newInputMovement.x;
            //newVelocity.z = newInputMovement.z;
            currentInputMovement = newInputMovement;
        }
        else
        {
            currentInputMovement = playerRigidBody.velocity;
        }

        //Decelerate
        if (isFlying) maxSpeed = 0f;
        newHorizontalSpeed = findHorizontalSpeed(newVelocity);
        if (newHorizontalSpeed > maxSpeed && !isWeightless)
        {
            float currentDeceleration = (isGrounded) ? groundDeceleration : airDeceleration;
            currentDeceleration = (isFlying) ? flyingDeceleration : currentDeceleration;
            Vector2 horizontalVelocity = new Vector2(newVelocity.x, newVelocity.z);
            Vector2 normalizedHorizontalVelocity = horizontalVelocity.normalized;
            horizontalVelocity.x -= (horizontalVelocity.x > 0) ? Mathf.Min((normalizedHorizontalVelocity.x * currentDeceleration), horizontalVelocity.x) :
                                                                 Mathf.Max((normalizedHorizontalVelocity.x * currentDeceleration), horizontalVelocity.x);
            horizontalVelocity.y -= (horizontalVelocity.y > 0) ? Mathf.Min((normalizedHorizontalVelocity.y * currentDeceleration), horizontalVelocity.y) :
                                                                 Mathf.Max((normalizedHorizontalVelocity.y * currentDeceleration), horizontalVelocity.y);
            newVelocity.x = horizontalVelocity.x;
            newVelocity.z = horizontalVelocity.y;
        }
        playerRigidBody.velocity = newVelocity;


        //Movement Stats
        currentVelocity = playerRigidBody.velocity;
        currentHorizontalSpeed = findHorizontalSpeed(currentVelocity);
        isMoving = movingThisUpdate;
    }


    //Sets Whether the Player is Rooted
    public void setPlayerRooted(bool rooted)
    {
        isRooted = rooted;
    }

    //Get the Magnitude of the x and z Elements of a Vector3
    private float findHorizontalSpeed(Vector3 velocity)
    {
        Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
        return horizontalVelocity.magnitude;
    }


    private IEnumerator jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            while (!isGrounded)
            {
                if (!Input.GetKey(KeyCode.Space))
                {
                    isJumping = false;
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            if (isJumping && isGrounded)
            {
                isJumping = true;
                isWeightless = true;
                playerRigidBody.velocity += Vector3.up * jumpSpeed;
                yield return new WaitForSeconds(0.1f);
                isWeightless = false;
                isJumping = false;
            }
        }
    }

}
