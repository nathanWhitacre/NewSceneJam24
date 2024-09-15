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


    // Start is called before the first frame update
    void Start()
    {
        isLocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked) return;

        float inputX = Input.GetAxis("Mouse X") * xLookSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * yLookSensitivity;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        playerTransform.Rotate(Vector3.up * inputX);
    }

    public void setLookLocked(bool locked)
    {
        isLocked = locked;
    }
}
