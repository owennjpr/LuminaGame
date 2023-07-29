using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fixedLightSpawner : MonoBehaviour
{
    
    private Vector3[] pathArray;
    public GameObject lightPrefab;
    public int colorID;
    private bool needToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        needToSpawn = true;
        switch (colorID) {
            case 0:
                pathArray = transform.parent.GetComponent<LightManager>().pathArray1;
                break;
            case 1:
                pathArray = transform.parent.GetComponent<LightManager>().pathArray2;
                break;
            case 2:
                pathArray = transform.parent.GetComponent<LightManager>().pathArray3;
                break;
            case 3:
                pathArray = transform.parent.GetComponent<LightManager>().pathArray4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needToSpawn) {
            needToSpawn = false;
            GameObject newLight = Instantiate(lightPrefab, transform.position, transform.rotation, transform);
            newLight.GetComponent<FloatingLightControl>().Init(pathArray, colorID);

        }
    }

    public void lightUsed() {
        needToSpawn = true;
    }
}
