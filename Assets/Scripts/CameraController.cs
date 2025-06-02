using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 1.0f;
    float rotationX = 0.0f;
    float rotationY = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // input from keyboard
        float forwardMove = Input.GetAxis("Vertical");   // WS (-1 to 1)
        float sidewaysMove = Input.GetAxis("Horizontal"); // AD (-1 to 1)
       
        // create a movement vector in local space
        Vector3 moveDirection = new Vector3(sidewaysMove, 0, forwardMove).normalized;

        // translate the camera in its local space
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);

        // rotation by mouse
        transform.localRotation = Quaternion.Euler(-rotationX, rotationX, 0);
        rotationY += Input.GetAxis("Mouse X") * rotateSpeed;
        rotationX -= Input.GetAxis("Mouse Y") * rotateSpeed;
        rotationX = Mathf.Clamp(rotationX, -20f, 50f); //control the vertical in range
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);


        // press esc to quit
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
