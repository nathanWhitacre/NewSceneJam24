using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraVertMovement : MonoBehaviour
{
    [SerializeField] private float ySensitivity;
    private Transform thisTrans;

    // Start is called before the first frame update
    void Start()
    {
        thisTrans = this.gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseVertical = Input.GetAxisRaw("Mouse Y") * -1 * ySensitivity;
        float targetVerticalAngle = mouseVertical + thisTrans.localEulerAngles.x;
        if (targetVerticalAngle > 90 && targetVerticalAngle < 180) {
            thisTrans.localEulerAngles = new Vector3(90, 0, 0);
        } else if (targetVerticalAngle > 180 && targetVerticalAngle < 270) {
            thisTrans.localEulerAngles = new Vector3(-90, 0, 0);
        }
        else {
            thisTrans.localEulerAngles = new Vector3(targetVerticalAngle, 0, 0);
        }
    }
}
