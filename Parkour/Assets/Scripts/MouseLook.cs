using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform Player;

    public float MouseSensitivity = 1f;
    public float YRotate;
    // Start is called before the first frame update
    void Start()
    {
        // makes sure the cursor isn't visible on screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.position;
        //Takes the movement of the mouse, multiplies it by the sensitivity 
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        // 

        //Y rotation needs to have a seperate variable to use Clamping to make sure the player can't rotate 360 degrees
        YRotate -= mouseY;
        YRotate = Mathf.Clamp(YRotate, -90f, 90f);
        //rotates the camera based on mouse y movement
        //doesn't rotate the player because that would mess with collision.
        transform.localRotation = Quaternion.Euler(YRotate, 0f, 0f);

        //rotates the player based on mouse X movement. 

        Player.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.up * mouseX);
        
        
    }

    
}
