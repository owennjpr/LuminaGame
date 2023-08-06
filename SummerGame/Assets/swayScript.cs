using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swayScript : MonoBehaviour
{
    private Vector3 startPos;
    private float time;
    private float idle_xMod;
    private float idle_yMod;
    private float idle_zMod;
    private float dist;

    private Rigidbody rb;

    private bool outOfRange;
    
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        idle_xMod = Random.Range(0.5f, 2.5f);
        idle_yMod = Random.Range(0.5f, 2.5f);
        idle_zMod = Random.Range(0.5f, 2.5f);
        time = 0;
        startPos = transform.position;
        outOfRange = false;

    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(startPos, transform.position);
        time += Time.deltaTime;

        // Debug.Log(dist);

        if (dist > 4) {
            outOfRange = true;
        } else if (dist < 0.5) {
            outOfRange = false;
        }

        if (outOfRange) {
            transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
        } else {
            
            float y = Mathf.Sin(time / idle_yMod);
            float x = Mathf.Sin(time / idle_xMod);
            float z = Mathf.Sin (time / idle_zMod);
            // transform.position = startPos + new Vector3(0.4f * x, 0.6f * y, 0.4f * z);
            rb.AddForce(new Vector3(2f * x, 1f * y, 2f * z));
        }

        Debug.Log(transform.rotation.eulerAngles.magnitude);

        if (transform.rotation.eulerAngles.magnitude > 45) {
            transform.Rotate(transform.rotation.eulerAngles * Time.deltaTime);
        }
    }
}
