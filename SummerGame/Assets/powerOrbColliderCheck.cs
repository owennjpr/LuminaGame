using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerOrbColliderCheck : MonoBehaviour
{
    
    [SerializeField] private GameObject magicCube;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("hi");
        if (other.CompareTag("Player")) {
            magicCube.GetComponent<magicCubeScript>().triggerStart();
        }
    }
}
