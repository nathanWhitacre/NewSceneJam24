using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody playerRigidBody;
    [SerializeField] private float topSpeed = 10f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float deceleration = 1f;
    public Vector3 currentVelocity;
    public float currentHorizontalSpeed;
    public Vector3 currentInputMovement;
    public bool isGrounded;
    public bool isMoving;
    public bool isRooted;

    public float lastX;
    public float lastZ;
    public Vector3 currentDecelerationStart;
    public bool isDecelerating;
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentInputMovement = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = transform.position + (Vector3.up * 10f);
            playerRigidBody.velocity += new Vector3(0f, 0f, 100f);
        }

    }

    private void FixedUpdate()
    {

        //RaycastHit groundHit;
        //if (Physics.Raycast(transform.position, Vector3.down, 0.1f, ))

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
        Vector3 newInputMovement = currentInputMovement;

        //Moving Forward
        if (Input.GetKey(KeyCode.W))
        {
            maxSpeed = topSpeed;
            newInputMovement += transform.forward * acceleration;
            movingThisUpdate = true;
        }

        //Moving Left
        if (Input.GetKey(KeyCode.A))
        {
            maxSpeed = topSpeed;
            newInputMovement += (transform.right * -1) * acceleration;
            movingThisUpdate = true;
        }

        //Moving Back
        if (Input.GetKey(KeyCode.S))
        {
            maxSpeed = topSpeed;
            newInputMovement += (transform.forward * -1) * acceleration;
            movingThisUpdate = true;
        }

        //Moving Right
        if (Input.GetKey(KeyCode.D))
        {
            maxSpeed = topSpeed;
            newInputMovement += transform.right * acceleration;
            movingThisUpdate = true;
        }

        if (movingThisUpdate)
        {
            //Clamp Top Movespeed
            float newInputSpeed = findHorizontalSpeed(newInputMovement);
            if (newInputSpeed > topSpeed)
            {
                newInputMovement.x = newInputMovement.x * (topSpeed / newInputSpeed);
                newInputMovement.z = newInputMovement.z * (topSpeed / newInputSpeed);
            }
            if (newHorizontalSpeed <= maxSpeed) newVelocity.x = newInputMovement.x;
            if (newHorizontalSpeed <= maxSpeed) newVelocity.z = newInputMovement.z;
            currentInputMovement = newInputMovement;
        }
        else
        {
            currentInputMovement = playerRigidBody.velocity;
        }

        //Decelerate
        newHorizontalSpeed = findHorizontalSpeed(newVelocity);
        if (newHorizontalSpeed > maxSpeed)
        {
            Vector2 horizontalVelocity = new Vector2(newVelocity.x, newVelocity.z);
            Vector2 normalizedHorizontalVelocity = horizontalVelocity.normalized;
            horizontalVelocity.x -= (horizontalVelocity.x > 0) ? Mathf.Min((normalizedHorizontalVelocity.x * deceleration), horizontalVelocity.x) :
                                                                 Mathf.Max((normalizedHorizontalVelocity.x * deceleration), horizontalVelocity.x);
            horizontalVelocity.y -= (horizontalVelocity.y > 0) ? Mathf.Min((normalizedHorizontalVelocity.y * deceleration), horizontalVelocity.y) :
                                                                 Mathf.Max((normalizedHorizontalVelocity.y * deceleration), horizontalVelocity.y);
            Debug.Log("normalizedHorizontalVelocity: " + normalizedHorizontalVelocity + "   |   deceleration: " + deceleration + "   |   mult: " + normalizedHorizontalVelocity * deceleration);
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

}
