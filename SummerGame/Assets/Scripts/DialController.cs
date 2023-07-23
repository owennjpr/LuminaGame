using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialController : MonoBehaviour
{
    
    public GameObject knobObject;
    public GameObject pointObject;
    public float distFromCenter;
    public float stepsize;
    private int knobMask;
    public int[] correctKnobValues;
    private int correctKnobMask;
    public int[] colorSequence;

    private bool solved;
    // Start is called before the first frame update
    void Start()
    {
        solved = false;
        knobMask = 0;
        for (int i = 0; i < 8; i++) {
            GameObject knob = Instantiate(knobObject, transform);
            knob.transform.position += new Vector3(0, distFromCenter, 0.18f);
            knob.transform.RotateAround(transform.position, new Vector3(0, 0, 1), 45 * i);
            knob.transform.RotateAround(transform.position, new Vector3(1, 0, 0), 45);

            knob.GetComponent<knobControl>().ID = i;
        }
        correctKnobMask = 0;
        foreach (int n in correctKnobValues) {
            int tempMask = 1 << n;
            correctKnobMask += tempMask;
        }
        


        int keyPointsLeft = colorSequence.Length;
        float distanceCounter = 0.0f;
        int counter = 0;
        while (keyPointsLeft > 0) {
            
            int angle = 45 * correctKnobValues[counter%correctKnobValues.Length];
            GameObject point = Instantiate(pointObject, transform);
            point.transform.position += new Vector3(0, distanceCounter, 0.075f);
            point.transform.RotateAround(transform.position, new Vector3(0, 0, 1), angle);
            point.transform.RotateAround(transform.position, new Vector3(1, 0, 0), 45);

            point.GetComponent<dialPoint>().colorID = colorSequence[counter];
            counter++;
            distanceCounter += stepsize;
            keyPointsLeft--;
        }

        // int numRandomPerKnob = colorSequence.Length/correctKnobValues.Length;
        // // Debug.Log(numRandomPerKnob);
        // for (int i = 0; i < 8; i++) {
        //     int tempMask = correctKnobMask & (1 << i);
        //     if ((tempMask) == 0) {
        //         Debug.Log("can fill " + i);
        //         PrevRange = 1;
        //         for (int j = 0; j < numRandomPerKnob; j++) {
        //             GameObject point = Instantiate(pointObject, transform);
        //             point.transform.position += new Vector3(0, Random.Range(1, 5)/10f, 0);
        //             point.transform.RotateAround(transform.position, new Vector3(0, 0, 1), 45*i);
        //         }
        //     }
        // }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!solved && correctKnobMask == knobMask) {
            solved = true;
            handleSolved();
            
        }
    }

    public void updateMask(bool activate, int knobID) {
        
        int tempMask = 1 << knobID;
        if (activate) {
            knobMask += tempMask;
        } else {
            knobMask -= tempMask;
        }
        Debug.Log("mask is " + knobMask);
    }

    private void handleSolved() {
        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("dial knob")) {
                // Debug.Log(child.GetComponent<knobControl>().ID);
                int tempMask = correctKnobMask & (1 << child.GetComponent<knobControl>().ID);
                if (tempMask != 0) {
                    child.GetComponent<knobControl>().triggerSolve();
                }
            }
        }
    }
}
