using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    [SerializeField] public Transform playerTransform;
    [SerializeField] public float xLookSensitivity = 1f;
    [SerializeField] public float yLookSensitivity = 1f;
    private float cameraVerticalRotation = 0f;
    public bool isLocked = false;
    public Vector3 eyePosition;
    public Vector3 facingDirection;


    // Start is called before the first frame update
    void Start()
    {
        isLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        if (isLocked) return;

        float inputX = Input.GetAxis("Mouse X") * xLookSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * yLookSensitivity;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        playerTransform.Rotate(Vector3.up * inputX);

        eyePosition = transform.position;
        facingDirection = transform.forward;
    }

    public void setLookLocked(bool locked)
    {
        isLocked = locked;
    }
}
