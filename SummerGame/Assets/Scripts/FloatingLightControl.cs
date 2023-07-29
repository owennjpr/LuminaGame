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
    public GameObject projectileToSpawn;
    private Transform controller;
    public float speed;
    private bool idle;
    private float time;
    private float idle_xMod;
    private float idle_yMod;
    private float idle_zMod;
    private int ID;

    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    
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


    public void Init(Vector3[] path, int lightID) {
        pathArray = path;
        // Debug.Log(lightID);
        ID = lightID;
        switch (ID) {
            case 0:
            transform.GetComponent<MeshRenderer> ().material = mat1;
                break;
            case 1:
            transform.GetComponent<MeshRenderer> ().material = mat2;
                break;
            case 2:
            transform.GetComponent<MeshRenderer> ().material = mat3;
                break;
            case 3:
            transform.GetComponent<MeshRenderer> ().material = mat4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1)) {
            beingPulled = false;
        }
        if (beingPulled) {
            idle = false;
            if (transform.position != playerHand.position) {
                transform.position = Vector3.MoveTowards(transform.position, playerHand.position, 15*Time.deltaTime);
            } 
            if (Vector3.Distance(transform.position, playerHand.position) < 0.25f) {
                if (Input.GetKeyDown("e")) {
                    // Debug.Log("E");
                    StartCoroutine(spawnLight());
                    
                } else if (Input.GetMouseButtonDown(0)) {
                    Debug.Log(ID);
                    switch(ID) {
                        case 0:
                            StartCoroutine(fireProjectile());
                            break;
                        case 1:
                            StartCoroutine(startSlowfall());
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
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

    private IEnumerator spawnLight() {
        
        GameObject newLight = Instantiate(objectToSpawn, transform.position, transform.rotation, controller);
        GetComponent<Renderer>().enabled = false;
        LightManager manager = transform.parent.GetComponent<LightManager>();
            if (manager != null) {
                manager.lightUsed(ID);
            } else {
                transform.parent.GetComponent<fixedLightSpawner>().lightUsed();
        }
        newLight.GetComponent<ControlledLightMove>().Init(pathArray, speed, ID);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private IEnumerator fireProjectile() {
        GameObject projectile = Instantiate(projectileToSpawn, transform.position, playerHand.rotation, controller);
        GetComponent<Renderer>().enabled = false;
        LightManager manager = transform.parent.GetComponent<LightManager>();
        if (manager != null) {
            manager.lightUsed(ID);
        } else {
            transform.parent.GetComponent<fixedLightSpawner>().lightUsed();
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);

    }

    private IEnumerator startSlowfall() {
        controller.GetComponent<GameController>().slowfall();
        while (transform.localScale.x > 0) {
            transform.localScale -= new Vector3(1, 1, 1) *Time.deltaTime;
            yield return null;
        }
        LightManager manager = transform.parent.GetComponent<LightManager>();
        if (manager != null) {
            manager.lightUsed(ID);
        } else {
            transform.parent.GetComponent<fixedLightSpawner>().lightUsed();
        }
        Destroy(gameObject);
    }
}
