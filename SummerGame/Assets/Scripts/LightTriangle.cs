using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTriangle : MonoBehaviour
{
    private Transform glowTri;
    private bool lit;
    public int localID;
    // Start is called before the first frame update
    void Start()
    {
        glowTri = transform.GetChild(0);
        lit = false;
        glowTri.gameObject.SetActive(false);
    }

    // public void hit() {
    //     Debug.Log("lamp smacked");
    //     if (!lit) {
    //         StartCoroutine(fillWithLight());
    //     } else {
    //         StartCoroutine(shrinkLight());
    //     }
    // }

    public IEnumerator fillWithLight() {
        Debug.Log("filling with light: " + localID);
        if (!lit) {
            glowTri.gameObject.SetActive(true);
            transform.parent.GetComponent<lampManager>().lampUpdated(localID, true);
            while(glowTri.localScale.x < 0.5f) {
                glowTri.localScale += new Vector3(0.5f, 0.5f, 0.5f) * 75 * Time.deltaTime;
                yield return null;
            }
            lit = true;
        }
        
    }
    public IEnumerator shrinkLight() {
        Debug.Log("shrinking light: " + localID);
        if (lit) {
            transform.parent.GetComponent<lampManager>().lampUpdated(localID, false);
            while(glowTri.localScale.x > 1f) {
                glowTri.localScale -= new Vector3(1f, 1f, 1f) * 75 * Time.deltaTime;
                yield return null;
            }
            glowTri.gameObject.SetActive(false);
            lit = false;
        }
    }
}
