using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCircle : MonoBehaviour
{
    private Transform glowCirc;
    private bool lit;
    public int localID;
    // Start is called before the first frame update
    void Start()
    {
        glowCirc = transform.GetChild(0);
        lit = false;
        glowCirc.gameObject.SetActive(false);
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
            glowCirc.gameObject.SetActive(true);
            transform.parent.GetComponent<lampManager>().lampUpdated(localID, true);
            while(glowCirc.localScale.x < 0.5f) {
                glowCirc.localScale += new Vector3(0.5f, 0.5f, 0.5f) * 75 * Time.deltaTime;
                yield return null;
            }
            lit = true;
        }
        
    }
    public IEnumerator shrinkLight() {
        Debug.Log("shrinking light: " + localID);
        if (lit) {
            transform.parent.GetComponent<lampManager>().lampUpdated(localID, false);
            while(glowCirc.localScale.x > 1f) {
                glowCirc.localScale -= new Vector3(1f, 1f, 1f) * 75 * Time.deltaTime;
                yield return null;
            }
            glowCirc.gameObject.SetActive(false);
            lit = false;
        }
    }
}
