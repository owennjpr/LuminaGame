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
    private bool idle;
    private float time;
    private float idle_xMod;
    private float idle_yMod;
    private float idle_zMod;
    
    // Start is called before the first frame update
    void Start()
    {
        idle_xMod = Random.Range(0.5f, 2.5f);
        idle_yMod = Random.Range(0.5f, 2.5f);
        idle_zMod = Random.Range(0.5f, 2.5f);
        idle = true;
        time = 0;
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
            idle = false;
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
            } else {
                idle = true;
                time = 0;
            }
        }
        if (idle) {
            // Debug.Log("I am chilling");
            time += Time.deltaTime;
            float y = Mathf.Sin(time / idle_yMod);
            float x = Mathf.Sin(time / idle_xMod);
            float z = Mathf.Sin (time / idle_zMod);
            transform.position = startPos + new Vector3(0.4f * x, 0.6f * y, 0.4f * z);
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
