using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialSphereIntersect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        if (transform.localScale.magnitude > 4) {
            Destroy(gameObject);
        } else {
            transform.localScale += Vector3.one * Time.deltaTime * 6;
        }
    }

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("Hit something");
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Dial Point")) {
            // Debug.Log("hit a point");
            dialPoint point = other.transform.GetComponent<dialPoint>();
            if (point.active) {
                Debug.Log(point.colorID);
            }
        }
    }
}
