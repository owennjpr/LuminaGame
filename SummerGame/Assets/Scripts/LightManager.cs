using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Vector3[] pathArray1;
    public Vector3[] pathArray2;
    public Vector3[] pathArray3;
    public Vector3[] pathArray4;
    private int[] currLightCounts;
    // public int activePaths;
    public bool[] activeLights;
    public int numEachLight;
    private int maxNumLights;
    private int currNumLights;
    public GameObject lightPrefab;
    public  CenterPointControl centerpoint;
    private Transform centerpointTrans;

    // Start is called before the first frame update
    void Start()
    {
        int counter = 0;
        for (int i = 0; i < 4; i++) {
            if (activeLights[i]) {
                counter++;
            }
        }
        maxNumLights = counter * numEachLight;
        centerpoint = transform.parent.GetComponent<CenterPointControl>();
        centerpointTrans = transform.parent;
        currLightCounts = new int[4];
        currLightCounts[0] = 0;
        currLightCounts[1] = 0;
        currLightCounts[2] = 0;
        currLightCounts[3] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currNumLights < maxNumLights) {
            currNumLights++;
            int index = 0;
            for (int i = 0; i < 4; i++) {
                if ((currLightCounts[i] < numEachLight ) && activeLights[i]) {
                    currLightCounts[i]++;
                    break;
                }
                index++;
            }
            float range = centerpoint.startDistance;
            Vector3 offset = new Vector3(Random.Range(-1 * range, range), Random.Range(2, 5), Random.Range(-1 * range, range));

            GameObject newLight = Instantiate(lightPrefab, centerpointTrans.position + offset, centerpointTrans.rotation, transform);
            switch (index) {
                case 0:
                    newLight.GetComponent<FloatingLightControl>().Init(pathArray1, index);
                    break;
                case 1:
                    newLight.GetComponent<FloatingLightControl>().Init(pathArray2, index);
                    break;
                case 2:
                    newLight.GetComponent<FloatingLightControl>().Init(pathArray3, index);
                    break;
                case 3:
                    newLight.GetComponent<FloatingLightControl>().Init(pathArray4, index);
                    break;
            }
        }
    }

    public void lightUsed(int lightID) {
        currNumLights--;
        currLightCounts[lightID]--;
    }
}
