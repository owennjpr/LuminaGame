using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    private float timer;
    public LayerMask ground;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * speed * 100);

    }
    //comments
    void Update() {
        timer += Time.deltaTime;
        if (timer >= 5) {
            Destroy(gameObject);

        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("breakable")) {
            other.GetComponent<Collider>().transform.GetComponent<breakableObject>().hit();
            StartCoroutine(handleImpact());
        } else if (other.CompareTag("lamp")) {
            Transform otherTransform = other.GetComponent<Collider>().transform;
            if (otherTransform.GetComponent<lamp>() != null) {
                otherTransform.GetComponent<lamp>().hit();
            } else {
                otherTransform.GetComponent<TimedLamp>().hit();
            }
            StartCoroutine(handleImpact());
        } else if (other.gameObject.layer == LayerMask.NameToLayer("ground")) {
            StartCoroutine(handleImpact());
        }
        
    }

    private IEnumerator handleImpact() {
        rb.velocity = new Vector3(0, 0, 0);
        rb.useGravity = false;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
 