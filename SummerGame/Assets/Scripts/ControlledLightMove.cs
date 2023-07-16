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
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Hello World");
        currIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(pathLength);
        if (currIndex <= pathLength) {
            transform.position = Vector3.MoveTowards(transform.position, currPoint, speed*Time.deltaTime);
            if (transform.position == currPoint) {
                currIndex++;
                Debug.Log("switching to point " + currIndex);
                if (currIndex == pathLength) {
                    Debug.Log("done");
                    Destroy(gameObject);
                } else {
                    currPoint = pathArray[currIndex];
                }
            }
        }
    }

    public void Init(Vector3[] inputPath, float movSpeed) {
        pathArray = inputPath;
        speed = movSpeed;
        pathLength = pathArray.Length;
        currPoint = pathArray[0];
    }

}
