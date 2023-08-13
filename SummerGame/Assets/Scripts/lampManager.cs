using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampManager : MonoBehaviour
{
    private int currMask;
    private int solutionMask;
    [SerializeField] private bool[] correctLamps;
    public GameObject solutionObject;
    [SerializeField] private bool verticalDoorSolve;
    [SerializeField] private bool revealLightSolve;
    [SerializeField] private float doorHeight;
    private bool hasBeenSolved;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenSolved = false;
        currMask = 0;
        solutionMask = 0;
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
