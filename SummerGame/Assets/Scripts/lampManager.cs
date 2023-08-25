using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampManager : MonoBehaviour
{
    //private not show up in unity can't be accessed anywhere else
    private int currMask;
    private int solutionMask;
    //[SerializeField] means shows up in unity and can't be accessed anywhere else
    [SerializeField] private bool[] correctLamps;
    public GameObject solutionObject;
    [SerializeField] private bool verticalDoorSolve;
    [SerializeField] private bool revealLightSolve;
    [SerializeField] private float doorHeight;
    private bool hasBeenSolved;
    [SerializeField] private bool isFivebyFive;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenSolved = false;
        currMask = 0;
        solutionMask = 0;
        //builds the solution mask to be used to check if correct
        for (int i = 0; i < correctLamps.Length; i++) {
            if (correctLamps[i]) {
                solutionMask += 1 << i;
            }
        }
        // Debug.Log(solutionMask);
    }

    public void lampUpdated(int lampID, bool activating) {
        if (activating) {
            currMask += 1 << lampID;
        } else {
            currMask -= 1 << lampID;
        }
        if (isFivebyFive && activating) { //only needs to check if new lamp has been turned on
            //check row repeats
            int row = lampID / 5;
            for (int i = 0; i < 5; i++) {
                if (((currMask & (1 << (row * 5 + i))) == 1) && ((row * 5 + i) != lampID)) { //should check if index i in row is on and also that on lamp was not just turned on
                    currMask -= 1 << (row * 5 + i);
                }
            }

            //check col repeats
            int col = lampID % 5;
            for (int j = 0; j < 5; j++) {
                if (((currMask & (1 << (5 * j + col))) == 1) && ((5 * j + col) != lampID)) { //should check if index i in row is on and also that on lamp was not just turned on
                    currMask -= 1 << (5 * j + col);
                }
            }

            //check group repeats
            //going to need to hard code these in somehow
            
        }

        if ((currMask == solutionMask) && !hasBeenSolved) {
            hasBeenSolved = true;
            Debug.Log("Correct Combination");
            if (verticalDoorSolve) {
                StartCoroutine(openVerticalDoor(doorHeight));
            } else if (revealLightSolve) {
                Debug.Log("revealing Light");
                solutionObject.SetActive(true);
            }
        }
    }

    private IEnumerator openVerticalDoor(float height) {
        Transform doorTransform = solutionObject.transform;
        Vector3 doorPosition = doorTransform.position;
        float counter = height;
        while (counter > 0.1) {
            counter -= Time.deltaTime;
            doorPosition -= new Vector3(0, Time.deltaTime, 0);
            doorTransform.position = doorPosition;
            yield return null;
        }
        
    }
}
