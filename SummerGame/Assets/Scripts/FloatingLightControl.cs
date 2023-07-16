using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLightControl : MonoBehaviour
{
    private Vector3 startPos;
    private bool beingPulled;
    private Transform playerHand;
    public Vector3[] pathArray;
    public GameObject objectToSpawn;
    private Transform controller;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindWithTag("GameController").transform;
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
            } else {
                if (Input.GetKeyDown("e")) {
                    Debug.Log("E");
                    StartCoroutine(spawnObject());
                    
                }
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

    private IEnumerator spawnObject() {
        
        GameObject newLight = Instantiate(objectToSpawn, transform.position, transform.rotation, controller);
        GetComponent<Renderer>().enabled = false;
        newLight.GetComponent<ControlledLightMove>().Init(pathArray, speed);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
