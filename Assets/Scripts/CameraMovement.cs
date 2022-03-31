using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    // World - Space
    float pitch = 0; // up or down
    float yaw = 0; // Side to side

    // Settings:
    public float lookSeneitivityX = 1;
    public float lookSeneitivityY = 1;

    void Start() {
        // Prevent Mouse from clicking outside of window while game is running 
        Cursor.lockState = CursorLockMode.Locked;   
    }
    void LateUpdate()  {
        // Get Mouse Input:
        float mx = Input.GetAxis("Mouse X"); // mx is number of pixels moved in x direction since last frame
        float my = Input.GetAxis("Mouse Y"); // my is Delta Y since last frame

        // Add change in mouse position to camera rotation values:
        yaw += mx * lookSeneitivityX;
        pitch += my * lookSeneitivityY; 
        pitch = Mathf.Clamp(pitch, -80, 80);

        // Update camera rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        if (target != null)
        {
            // Follow Target
            //transform.position = AnimMath.Ease(transform.position, target.position, .001f);   //EASE TO PLAYER
            transform.position = target.position;                                               // SNAP TO PLAYER
        }
    }
}
