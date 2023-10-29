using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    float xRot;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
        {
            xRot += mouseY;
        }
        else
        {
            xRot -= mouseY;
        }


        xRot = Mathf.Clamp(xRot, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
