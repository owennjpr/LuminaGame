using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBackForth : MonoBehaviour
{
    private float zPos;
    private bool forward;
    // Start is called before the first frame update
    void Start()
    {
        zPos = 15;
        forward = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3 (0, 2, zPos);
        if (forward) {
            zPos += 0.1f;
        } else {
            zPos -= 0.1f;
        }
        if (zPos < 15) {
            forward = true;
        } else if (zPos >= 60) {
            forward = false;
        }
        
        

    }
}
