using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    
    public float sensX;
    public float sensY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;


    public float xOverride;
    public float yOverride;

    public bool paused;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        xOverride = 0f;
        yOverride = 180f;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX, mouseY;
        if (paused) {
            mouseX = 0f;
            mouseY = 0f;
        } else {
            mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX;
            mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;
        }
        yRotation += mouseX;
        xRotation -= mouseY;
        // Debug.Log(yRotation);        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation + xOverride, yRotation + yOverride, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation + yOverride, 0);
    }
}
