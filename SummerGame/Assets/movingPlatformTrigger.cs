using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformTrigger : MonoBehaviour
{

    public Vector3 currforce;
    public Rigidbody targetrb; 
    // Start is called before the first frame update
    void Start()
    {
        currforce = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetrb != null) {
            Debug.Log(currforce);
            targetrb.AddForce(currforce);

        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            targetrb = other.GetComponent<Rigidbody>();
            Debug.Log("YAYAYAYYA");
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            targetrb = null;
            Debug.Log("ByeBye");
        }
    }
}
