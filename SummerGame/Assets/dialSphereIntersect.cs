using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialSphereIntersect : MonoBehaviour
{
    private List<int> colorSequence;
    private GameController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        colorSequence = new List<int>();
        controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        if (transform.localScale.magnitude > 2.5f) {
            controller.showColorSequence(colorSequence);
            Destroy(gameObject);
        } else {
            transform.localScale += Vector3.one * Time.deltaTime * 10;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Dial Point")) {
            dialPoint point = other.transform.GetComponent<dialPoint>();
            if (point.active) {
                // Debug.Log(point.colorID);
                colorSequence.Add(point.colorID);

            }
        }
    }
}
