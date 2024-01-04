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
    [SerializeField] private bool isLogicGate;

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

        if (isFivebyFive) {
            StartCoroutine(transform.GetChild(0).GetComponent<lamp>().fillWithLight());
            Debug.Log(currMask);
        }
        // Debug.Log(solutionMask);
    }

    public void lampUpdated(int lampID, bool activating) {
        if (activating) {
            currMask += 1 << lampID; //what happens if trying to activate one that's already on?
            Debug.Log(lampID);
        } else {
            currMask -= 1 << lampID;
        }
        if (isFivebyFive && activating) { //only needs to check if new lamp has been turned on
            //check row repeats
            Debug.Log("is 5x5 and activating");
            int row = lampID / 5;
            int tempmask = currMask;
            for (int i = 0; i < 5; i++) {
                // Debug.Log("in for loop row");
                if (((tempmask & (1 << (row * 5 + i))) == (1 << (row * 5 + i))) && ((row * 5 + i) != lampID)) { //should check if index i in row is on and also that on lamp was not just turned on
                    Debug.Log((row * 5 + i));
                    Debug.Log("2: " + (1 << (row * 5 + i)));
                    StartCoroutine(transform.GetChild((row * 5 + i)).GetComponent<lamp>().shrinkLight());
                    Debug.Log("found two row");
                }
            }

            Debug.Log(currMask);

            //check col repeats
            int col = lampID % 5;
            for (int j = 0; j < 5; j++) {
                if (((tempmask & (1 << (5 * j + col))) == (1 << (5 * j + col))) && ((5 * j + col) != lampID)) { //should check if index i in row is on and also that on lamp was not just turned on
                    StartCoroutine(transform.GetChild((5 * j + col)).GetComponent<lamp>().shrinkLight());
                    Debug.Log("found two col");
                }
            }
            Debug.Log(currMask);
            tempmask = currMask;

            //check group repeats, don't need to check the group of 1
            // if (((tempmask & (1 << 19)) == (1 << 19)) && ((tempmask & (1 << 24)) == (1 << 24))) {
            //     if (19 != lampID) {
            //         StartCoroutine(transform.GetChild(19).GetComponent<lamp>().shrinkLight());
            //     } else {
            //         StartCoroutine(transform.GetChild(24).GetComponent<lamp>().shrinkLight());
            //     }
            // }

            //THIS PART IS NOT WORKING RN

            // int[][] all = new int[4][];

            // // Initialize the elements.
            // arr[0] = new int[5] { 1, 3, 5, 7, 9 };
            // arr[1] = new int[4] { 2, 4, 6, 8 };

            //IDEA:   COULD MAKE ONE BIG ARRAY BC WOULD KNOW INDEX OF START OF EACH ONE WHICH COULD BE STORED IN ANOTHER ARRAY
            // group0 just has index 0 in it because it just has the one
            int[] group1 = {1, 2, 6, 7, 11, 12, 13, 16, 17, 18, 22, 23};
            int[] group2 = {3, 4, 8, 9, 14};
            int[] group3 = {5, 10, 15, 20, 21};
            int[] group4 = {19, 24};

            // int* all = {group1, group2, group3, group4};

            // for (int group = 1; group < 5; group++) {
                // Debug.Log(all[group][0][0]);
                // string name = ("group" + group);
            for (int i = 0; i < group1.Length; i++) {
                for (int j = i + 1; j < group1.Length; j++) {
                    Debug.Log("GROUP 1: " + group1[i] + ", " + group1[j]);
                    if ((group1[i] != group1[j]) && ((tempmask & (1 << group1[i])) == (1 << group1[i])) && ((tempmask & (1 << group1[j])) == (1 << group1[j]))) {
                         Debug.Log("TWOOO IN GROUP 1: " + group1[i] + ", " + group1[j]);
                        if (group1[i] != lampID) {
                            StartCoroutine(transform.GetChild(group1[i]).GetComponent<lamp>().shrinkLight());
                        } else {
                            StartCoroutine(transform.GetChild(group1[j]).GetComponent<lamp>().shrinkLight());
                        }
                    }
                }
            }
            // }

            for (int i = 0; i < group2.Length; i++) {
                for (int j = 0; j < group2.Length; j++) {
                    //Debug.Log(group2[i] + ", " + group2[j]);
                    if ((group2[i] != group2[j]) && ((tempmask & (1 << group2[i])) == (1 << group2[i])) && ((tempmask & (1 << group2[j])) == (1 << group2[j]))) {
                        // Debug.Log(group2[i] + ", " + group2[j]);
                        if (group2[i] != lampID) {
                            StartCoroutine(transform.GetChild(group2[i]).GetComponent<lamp>().shrinkLight());
                        } else {
                            StartCoroutine(transform.GetChild(group2[j]).GetComponent<lamp>().shrinkLight());
                        }
                    }
                }
            }

            for (int i = 0; i < group3.Length; i++) {
                for (int j = 0; j < group3.Length; j++) {
                    //Debug.Log(group3[i] + ", " + group3[j]);
                    if ((group3[i] != group3[j]) && ((tempmask & (1 << group3[i])) == (1 << group3[i])) && ((tempmask & (1 << group3[j])) == (1 << group3[j]))) {
                        // Debug.Log(group2[i] + ", " + group2[j]);
                        if (group3[i] != lampID) {
                            StartCoroutine(transform.GetChild(group3[i]).GetComponent<lamp>().shrinkLight());
                        } else {
                            StartCoroutine(transform.GetChild(group3[j]).GetComponent<lamp>().shrinkLight());
                        }
                    }
                }
            }

            for (int i = 0; i < group4.Length; i++) {
                for (int j = 0; j < group4.Length; j++) {
                    //Debug.Log(group4[i] + ", " + group4[j]);
                    if ((group4[i] != group4[j]) && ((tempmask & (1 << group4[i])) == (1 << group4[i])) && ((tempmask & (1 << group4[j])) == (1 << group4[j]))) {
                        // Debug.Log(group2[i] + ", " + group2[j]);
                        if (group4[i] != lampID) {
                            StartCoroutine(transform.GetChild(group4[i]).GetComponent<lamp>().shrinkLight());
                        } else {
                            StartCoroutine(transform.GetChild(group4[j]).GetComponent<lamp>().shrinkLight());
                        }
                    }
                }
            }

            //going to need to hard code these in somehow
            
        }

        if (isLogicGate && activating) { //only needs to check if new lamp has been turned on
            if (lampID == 0 || lampID == 1) {
                if (((currMask & 2) == 2) && (currMask & 1) == 0) {
                    StartCoroutine(transform.GetChild(9).GetComponent<lamp>().fillWithLight());
                }

            } else if (lampID == 2 || lampID == 3) {
                if (((currMask & (1 << 2)) == (1 << 2)) && (currMask & (1 << 3)) == 0) {
                    StartCoroutine(transform.GetChild(10).GetComponent<lamp>().fillWithLight());
                }

            } else if (lampID == 4 || lampID == 5) {
                if (((currMask & (1 << 4)) == (1 << 4)) && (currMask & (1 << 5)) == 0) {
                    StartCoroutine(transform.GetChild(11).GetComponent<lamp>().fillWithLight());
                }

            }
            if ((currMask & (7 << 9)) == (7 << 9)) {
                StartCoroutine(transform.GetChild(12).GetComponent<lamp>().fillWithLight());
            }

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
