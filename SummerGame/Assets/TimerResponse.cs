using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerResponse : MonoBehaviour
{
    private Rigidbody rb;
    
    //for vertical moving platforms
    public bool isElevator;
    public float riseSpeed;
    public float maxHeight;
    public float minHeight;
    private bool rising;

    // Start is called before the first frame update
    void Start()
    {
        rising = false;
        rb = transform.GetComponent<Rigidbody>();
    }

    public void activate() {
        if (isElevator) {
            rising = true;
            rb.velocity = Vector3.up * riseSpeed;
            Debug.Log("activated");
        }
    }

    public void deactivate() {
        if (isElevator) {
            rising = false;
            rb.velocity = Vector3.down * riseSpeed;
            Debug.Log("deactivated");
        }
    }

    void Update() {
        if (rising && maxHeight <= transform.localPosition.y) {
            rb.velocity = Vector3.zero;
        } else if (!rising && transform.localPosition.y <= minHeight) {
            rb.velocity = Vector3.zero;
        }
    }
}
