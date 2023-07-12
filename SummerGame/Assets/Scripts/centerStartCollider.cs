using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerStartCollider : MonoBehaviour
{
    public CenterPointControl control;
    // Start is called before the first frame update
    void Start()
    {
        control = transform.parent.GetComponent<CenterPointControl>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("entering");
            control.updateCenterState();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("exiting");
            control.updateCenterState();
        }
    }
}
