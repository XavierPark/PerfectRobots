using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMove : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int vertMin;
    [SerializeField] int vertMax;

    [SerializeField] bool invert_y;

    float xRot;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        //get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;


        if (invert_y)
            xRot += mouseY;
        else
            xRot -= mouseY;

        //clamp the rot on the x axis
        xRot = Mathf.Clamp(xRot, vertMin, vertMax);

        //rotate the camera on the x axis

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        //rotate the player on the y axis
        transform.parent.Rotate(Vector3.up * mouseX);

    }

}
