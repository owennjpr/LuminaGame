using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformTrigger : MonoBehaviour
{

    public Transform targetTransform; 
    // Start is called before the first frame update
    

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            targetTransform = other.transform;
            targetTransform.SetParent(transform.parent, true);
            Debug.Log("YAYAYAYYA");
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            targetTransform.SetParent(null, true);
            targetTransform = null;
            Debug.Log("ByeBye");
        }
    }
}
