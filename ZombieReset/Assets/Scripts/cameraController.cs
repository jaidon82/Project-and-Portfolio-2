using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;



    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState= CursorLockMode.Locked;
        transform.forward= transform.parent.forward;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY= Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;


        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;



        //x-axis Clamp
        rotX= Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //y-axis Clamp
        transform.parent.Rotate(Vector3.up * mouseX);

        //x-axis rotation
        transform.localRotation= Quaternion.Euler(rotX,0,0);
    }
}
