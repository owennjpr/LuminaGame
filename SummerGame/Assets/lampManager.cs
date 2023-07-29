using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampManager : MonoBehaviour
{
    private int currMask;
    private int solutionMask;
    [SerializeField] private bool[] correctLamps;
    
    // Start is called before the first frame update
    void Start()
    {
        currMask = 0;
        solutionMask = 0;
        for (int i = 0; i < correctLamps.Length; i++) {
            if (correctLamps[i]) {
                solutionMask += 1 << i;
            }
        }
        Debug.Log(solutionMask);
    }

    public void lampUpdated(int lampID, bool activating) {
        if (activating) {
            currMask += 1 << lampID;
        } else {
            currMask -= 1 << lampID;
        }
        if (currMask == solutionMask) {
            Debug.Log("Correct Combination");
        }
    }
}
