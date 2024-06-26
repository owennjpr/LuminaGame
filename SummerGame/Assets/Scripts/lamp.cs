using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lamp : MonoBehaviour
{
    private Transform glowCube;
    private bool lit;
    public int localID;
    // Start is called before the first frame update
    void Start()
    {
        glowCube = transform.GetChild(0);
        lit = false;
        glowCube.gameObject.SetActive(false);
    }

    public void hit() {
        Debug.Log("lamp smacked");
        if (!lit) {
            StartCoroutine(fillWithLight());
        } else {
            StartCoroutine(shrinkLight());
        }
    }

    public IEnumerator fillWithLight() {
        Debug.Log("filling with light: " + localID);
        if (!lit) {
            glowCube.gameObject.SetActive(true);
            transform.parent.GetComponent<lampManager>().lampUpdated(localID, true);
            while(glowCube.localScale.x < 17f) {
                glowCube.localScale += new Vector3(1f, 1f, 1f) * 75 * Time.deltaTime;
                yield return null;
            }
            lit = true;
        }
        
    }
    public IEnumerator shrinkLight() {
        Debug.Log("shrinking light: " + localID);
        if (lit) {
            transform.parent.GetComponent<lampManager>().lampUpdated(localID, false);
            while(glowCube.localScale.x > 1f) {
                glowCube.localScale -= new Vector3(1f, 1f, 1f) * 75 * Time.deltaTime;
                yield return null;
            }
            glowCube.gameObject.SetActive(false);
            lit = false;
        }
    }
}
