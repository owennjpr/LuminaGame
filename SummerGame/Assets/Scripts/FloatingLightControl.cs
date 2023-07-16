using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLightControl : MonoBehaviour
{
    private Vector3 startPos;
    private bool beingPulled;
    private Transform playerHand;
    public Vector3[] pathArray;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        playerHand = GameObject.FindWithTag("Player").transform.GetChild(0).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1)) {
            beingPulled = false;
        }
        if (beingPulled) {
            // Debug.Log("Bam"); 
            if (transform.position != playerHand.position) {
                // Debug.Log("en route");
                transform.position = Vector3.MoveTowards(transform.position, playerHand.position, 15*Time.deltaTime);
            }
        } else {
            if (transform.position != startPos) {
                transform.position = Vector3.MoveTowards(transform.position, startPos, 3*Time.deltaTime);
            }

        }
    }

    public void playerClicked() {
        beingPulled = true;
    }
}
