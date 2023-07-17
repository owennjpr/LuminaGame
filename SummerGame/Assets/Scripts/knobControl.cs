using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knobControl : MonoBehaviour
{
    private bool active;
    public Vector3 center;
    private LayerMask mask;
    private float distFromCenter;
    private Vector3 direction;
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        center = transform.parent.position;
        distFromCenter = Vector3.Distance(transform.position, center);
        direction = center - transform.position;
        mask = 1 << 8;
    }

    // Update is called once per frame
    void Update()
    {
        // if (active) {
        //     RaycastHit hit;
        //     if (Physics.Raycast(transform.position, direction, out hit, distFromCenter -0.1f, mask)) {
        //         Debug.Log(hit.collider.gameObject.name + " was hit");
                
        //     }
        //     Debug.DrawRay(transform.position, direction * (distFromCenter-0.1f), Color.green);
        // }
    }

    public void clicked() {
        active = !active;
        if (active) {
            activate();
        } else {
            deactivate();
        }
    }

    private void activate() {
        // Debug.Log("activating");
        transform.position = Vector3.MoveTowards(transform.position, center, 0.1f);
        transform.parent.GetComponent<DialController>().updateMask(true, ID);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, distFromCenter -0.1f, mask);

        foreach(RaycastHit h in hits) {
            // Debug.Log(h.collider.gameObject.name + " was hit");
            h.collider.gameObject.GetComponent<dialPoint>().activate();
        }
        // Debug.DrawRay(transform.position, direction * (distFromCenter-0.1f), Color.green);
    }

    private void deactivate() {
        // Debug.Log("deactivating");
        transform.position = Vector3.MoveTowards(transform.position, center, -0.1f);
        transform.parent.GetComponent<DialController>().updateMask(false, ID);

        
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, distFromCenter, mask);

        foreach(RaycastHit h in hits) {
            // Debug.Log(h.collider.gameObject.name + " was hit");
            h.collider.gameObject.GetComponent<dialPoint>().deactivate();
        }
        // Debug.DrawRay(transform.position, direction * (distFromCenter), Color.green);
    }

    public void triggerSolve() {
        Debug.Log(ID);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, direction, distFromCenter -0.1f, mask);

        foreach(RaycastHit h in hits) {
            h.collider.gameObject.GetComponent<dialPoint>().solve();
        }
    }
}
