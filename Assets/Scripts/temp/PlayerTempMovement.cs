using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTempMovement : MonoBehaviour
{
    private Rigidbody thisBody;
    private Transform thisTrans;
    [SerializeField] private float speed;
    [SerializeField] private float xSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        thisBody = this.gameObject.GetComponent<Rigidbody>();
        thisTrans = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseHorizontal = Input.GetAxisRaw("Mouse X") * xSensitivity;
        thisTrans.localEulerAngles += new Vector3(0, mouseHorizontal, 0);

        thisBody.angularVelocity = Vector3.zero;
    }

    void FixedUpdate() {
        Vector3 vel = thisBody.velocity;
        
        Vector3 moveFlatDir = Vector3.zero;
        moveFlatDir += Input.GetAxisRaw("Horizontal") * thisTrans.right;
        moveFlatDir += Input.GetAxisRaw("Vertical") * thisTrans.forward;
        moveFlatDir = moveFlatDir.normalized * speed;

        vel.x = moveFlatDir.x;
        vel.z = moveFlatDir.z;

        thisBody.velocity = vel;
    }
}
