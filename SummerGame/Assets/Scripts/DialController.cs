using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialController : MonoBehaviour
{
    private struct pointData {
        public int[] positions;
        public int[] colors;
    };

    public GameObject knobObject;
    public GameObject pointObject;
    public float distFromCenter;
    [SerializeField] private float stepsize;
    private int knobMask;
    [SerializeField] private int[] correctKnobValues;
    private int correctKnobMask;
    [SerializeField] private int[] colorSequence;
    private bool solved;
    private Vector3 centerPos;

    [SerializeField] private int[] knob0_positions;
    [SerializeField] private int[] knob0_colors;

    [SerializeField] private int[] knob1_positions;
    [SerializeField] private int[] knob1_colors;

    [SerializeField] private int[] knob2_positions;
    [SerializeField] private int[] knob2_colors;

    [SerializeField] private int[] knob3_positions;
    [SerializeField] private int[] knob3_colors;

    [SerializeField] private int[] knob4_positions;
    [SerializeField] private int[] knob4_colors;

    [SerializeField] private int[] knob5_positions;
    [SerializeField] private int[] knob5_colors;

    [SerializeField] private int[] knob6_positions;
    [SerializeField] private int[] knob6_colors;

    [SerializeField] private int[] knob7_positions;
    [SerializeField] private int[] knob7_colors;


    // Start is called before the first frame update
    void Start()
    {
        solved = false;
        knobMask = 0;
        correctKnobMask = 0;
        foreach (int n in correctKnobValues) {
            int tempMask = 1 << n;
            correctKnobMask += tempMask;
        }
        
        centerPos = transform.position + new Vector3(0, -0.1f, 0.1f);

        // int keyPointsLeft = colorSequence.Length;
        // float distanceCounter = 0.0f;
        // int counter = 0;
        // while (keyPointsLeft > 0) {
            
        //     int angle = 45 * correctKnobValues[counter%correctKnobValues.Length];
        //     GameObject point = Instantiate(pointObject, transform);
        //     point.transform.position += new Vector3(0, distanceCounter, 0.075f);
        //     point.transform.RotateAround(centerPos, new Vector3(0, 0, 1), angle);
        //     point.transform.RotateAround(centerPos, new Vector3(1, 0, 0), 45);

        //     point.GetComponent<dialPoint>().colorID = colorSequence[counter];
        //     counter++;
        //     distanceCounter += stepsize;
        //     keyPointsLeft--;
        // }

        for (int i = 0; i < 8; i++) {
            pointData currKnobData = getActiveKnobData(i);
            int numPoints = currKnobData.positions.Length;
            for (int j = 0; j < numPoints; j++) {
                GameObject point = Instantiate(pointObject, transform);
                point.transform.position += new Vector3(0, -0.1f, 0.1f);
                point.transform.position += new Vector3(0, stepsize * currKnobData.positions[j], -0.05f);
                point.transform.RotateAround(centerPos, new Vector3(0, 0, 1), 45*i);
                point.transform.RotateAround(centerPos, new Vector3(1, 0, 0), 45);


            }
        }
    }

    private pointData getActiveKnobData(int i) {
        pointData currKnobData = new pointData();
        switch (i) {
            case 0:
                currKnobData.positions = knob0_positions;
                currKnobData.colors = knob0_colors;
                break;
            case 1:
                currKnobData.positions = knob1_positions;
                currKnobData.colors = knob1_colors;
                break;
            case 2:
                currKnobData.positions = knob2_positions;
                currKnobData.colors = knob2_colors;
                break;
            case 3:
                currKnobData.positions = knob3_positions;
                currKnobData.colors = knob3_colors;
                break;
            case 4:
                currKnobData.positions = knob4_positions;
                currKnobData.colors = knob4_colors;
                break;
            case 5:
                currKnobData.positions = knob5_positions;
                currKnobData.colors = knob5_colors;
                break;
            case 6:
                currKnobData.positions = knob6_positions;
                currKnobData.colors = knob6_colors;
                break;
            case 7:
                currKnobData.positions = knob7_positions;
                currKnobData.colors = knob7_colors;
                break;
        }
        return currKnobData;

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
