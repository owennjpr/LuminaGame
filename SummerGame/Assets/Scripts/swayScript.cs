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
    private Vector3 currForce;
    public movingPlatformTrigger childCollider;

    public float multiplier;
    
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        idle_xMod = Random.Range(0.5f, 2f);
        idle_yMod = Random.Range(0.5f, 1.5f);
        idle_zMod = Random.Range(0.5f, 2f);
        time = Random.Range(0, 30);
        startPos = transform.position;
        outOfRange = false;
        currForce = new Vector3(0, 0, 0);
        childCollider = transform.GetChild(0).GetComponent<movingPlatformTrigger>();

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
            currForce = (startPos - transform.position).normalized;
        } else {
            
            float y = Mathf.Sin(time / idle_yMod);
            float x = Mathf.Sin(time / idle_xMod);
            float z = Mathf.Sin (time / idle_zMod);
        // currTransform = new Vector3(x, y, z);
        // transform.position = startPos + currTransform;
            currForce = new Vector3(2f * x, 1f * y, 2f * z);
            // childCollider.currforce = currforce;
            
        }

        rb.velocity = currForce * multiplier;

    }

    // private void OnTriggerStay(Collider other) {
    //     if (other.gameObject.tag == "Player") {
    //         Debug.Log("YAYAYAYYA");
    //     }
    // }
}
