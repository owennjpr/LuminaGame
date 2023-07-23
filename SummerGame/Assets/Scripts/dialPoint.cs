using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialPoint : MonoBehaviour
{
    public bool active;
    public Material activeMat;
    public Material inactiveMat;

    public Material color0;
    public Material color1;
    public Material color2;
    public Material color3;

    public int colorID;
    private int rotateSpeed;
    private bool solved;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        solved = false;
        rotateSpeed = 50;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate() {
        if (!solved) {
            active = true;
            transform.GetComponent<MeshRenderer> ().material = activeMat;
        }
    }
    public void deactivate() {
        if (!solved) {
            active = false;
            // Debug.Log("point deactivating");
            transform.GetComponent<MeshRenderer> ().material = inactiveMat;
        }
        
    }

    public void solve() {
        // Debug.Log("hi there");
        solved = true;
        switch (colorID) {
            case 0:
                transform.GetComponent<MeshRenderer> ().material = color0;
                break;
            case 1:
                transform.GetComponent<MeshRenderer> ().material = color1;
                break;
            case 2:
                transform.GetComponent<MeshRenderer> ().material = color2;
                break;
            case 3:
                transform.GetComponent<MeshRenderer> ().material = color3;
                break;
        }
        StartCoroutine(rotateToAngle(270));

        // StartCoroutine(popOut(1));
    }

    private IEnumerator rotateToAngle(float target) {
        float start = transform.eulerAngles.z;
        if (start < target) {
             while(start < target) {
                start += rotateSpeed*Time.deltaTime;
                transform.RotateAround(transform.parent.position, new Vector3(1, 0, 1), rotateSpeed*Time.deltaTime);
                yield return null;
            }
        } else {
            while(start > target) {
                start -= rotateSpeed*Time.deltaTime;
                transform.RotateAround(transform.parent.position, new Vector3(1, 0, 1), -1*rotateSpeed*Time.deltaTime);
                yield return null;
            }
        }
    }
    // private IEnumerator popOut(float distance) {
    //     float curr = 0;
    //     while (curr < distance) {

    //     }
    // }
}
