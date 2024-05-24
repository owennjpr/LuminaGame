using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledLightMove : MonoBehaviour
{
    private Vector3[] pathArray;
    private int pathLength;
    private int currIndex;
    private Vector3 currPoint;
    private float speed;

    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    private int colorId;

    private bool insideObject;
    public GameObject particleObj;

    public LayerMask groundLayer;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log("Hello World");
        currIndex = 0;
        insideObject = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currIndex <= pathLength) {
            transform.position = Vector3.MoveTowards(transform.position, currPoint, speed*Time.deltaTime);
            if (transform.position == currPoint) {
                currIndex++;
                // Debug.Log("switching to point " + currIndex);
                if (currIndex == pathLength) {
                    // Debug.Log("done");
                    Destroy(gameObject);
                } else {
                    currPoint = pathArray[currIndex];
                }
            }
        }

        bool objCheck = Physics.CheckSphere(transform.position, 0.1f, groundLayer);
        if (objCheck && !insideObject) {
            instantiateParticles();
            insideObject = true;
        } else if (!objCheck && insideObject) {
            instantiateParticles();
            insideObject = false;
        }
        
    }

    private void instantiateParticles() {
        GameObject particles = Instantiate(particleObj, transform.position, Quaternion.identity);
        particles.GetComponent<particleSelfDestruct>().setColor(colorId);
    }

    public void Init(Vector3[] inputPath, float movSpeed, int lightID) {
        
        // Debug.Log("initializing light");
        pathArray = inputPath;
        speed = movSpeed;
        pathLength = pathArray.Length;
        currPoint = pathArray[0];
        colorId = lightID;
        switch (lightID) {
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

    public Vector3 getDestination() {
        
        Debug.Log("Getting destination");
        
        return pathArray[pathLength-1];
    }

    // void OnTriggerEnter(Collider other) {
    //     Debug.Log(other.gameObject.name);   
    //     if (!insideObject && ! other.isTrigger && other.gameObject.tag != "Player") {
    //         insideObject = true;
    //         Instantiate(particleObj, transform.position, Quaternion.identity);
    //     }
         

    // }

    // void OnTriggerExit(Collider other) {
    //     if (insideObject && ! other.isTrigger && other.gameObject.tag != "Player") {
    //         insideObject = false;
    //         Instantiate(particleObj, transform.position, Quaternion.identity);
    //     }
    // }

}
