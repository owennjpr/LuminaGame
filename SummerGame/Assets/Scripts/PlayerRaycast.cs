using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{   
    private Camera mainCam;
    public LayerMask mask;
    public Image crosshair;
    private Color active;
    private Color inactive;
    private float rotation;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        active = Color.white;
        inactive = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 15, mask)) {
            if(Input.GetMouseButtonDown(0)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Dial Control")) {
                    hit.collider.gameObject.GetComponent<knobControl>().clicked();
                }
            }
            
            if (Input.GetMouseButtonDown(1)) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Floating Light")) {
                    hit.collider.gameObject.GetComponent<FloatingLightControl>().playerClicked();
                }
            }
            rotation += 90 *Time.deltaTime;
            crosshair.rectTransform.localEulerAngles = new Vector3(0, 0, rotation);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            // Debug.Log("Did Hit");
            crosshair.color = active;
            crosshair.rectTransform.localScale = new Vector3(2, 2, 1);
        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            crosshair.color = inactive;
            crosshair.rectTransform.localScale = new Vector3(1, 1, 1);
            rotation = 0;
            crosshair.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
