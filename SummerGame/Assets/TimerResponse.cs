using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerResponse : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("ELEVATOR")]
    //for vertical moving platforms
    public bool isElevator;
    public float riseSpeed;
    public float maxHeight;
    public float minHeight;
    private bool rising;
    
    [Header("DOOR")]
    public bool isVerticalDoor;
    public float doorHeight;
    public float speed;
    private bool opening;
    private bool closing;
    
    

    // Start is called before the first frame update
    void Start()
    {
        if (isElevator) {
            rising = false;
            rb = transform.GetComponent<Rigidbody>();   
        }
    }

    public void activate() {
        if (isElevator) {
            rising = true;
            rb.velocity = Vector3.up * riseSpeed;
            Debug.Log("activated");
        } else if (isVerticalDoor) {
            StartCoroutine(openVerticalDoor());
        }
    }

    public void deactivate() {
        if (isElevator) {
            rising = false;
            rb.velocity = Vector3.down * riseSpeed;
            Debug.Log("deactivated");
        } else if (isVerticalDoor) {
            StartCoroutine(closeVerticalDoor());
        }
    }

    void Update() {
        if (isElevator) {
            if (rising && maxHeight <= transform.localPosition.y) {
                rb.velocity = Vector3.zero;
            } else if (!rising && transform.localPosition.y <= minHeight) {
                rb.velocity = Vector3.zero;
            }
        } 
    }

    private IEnumerator openVerticalDoor() {
        gameObject.GetComponent<AudioSource>().Play();
        opening = true;
        Vector3 doorPosition = transform.position;
        while (doorPosition.y > - doorHeight/2) {
            doorPosition -= new Vector3(0, Time.deltaTime * speed, 0);
            transform.position = doorPosition;
            yield return null;
        }
        opening = false;
        
    }

    private IEnumerator closeVerticalDoor() {
        // gameObject.GetComponent<AudioSource>().Play();
        closing = true;
        Vector3 doorPosition = transform.position;
        while (doorPosition.y < doorHeight/2) {
            if (opening) {
                break;
            }
            doorPosition += new Vector3(0, Time.deltaTime * speed, 0);
            transform.position = doorPosition;
            yield return null;
        }
        
        closing = false;
    }
}
