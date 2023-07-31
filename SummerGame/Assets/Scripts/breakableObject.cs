using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableObject : MonoBehaviour
{
    private Transform glowCube;
    // Start is called before the first frame update
    void Start()
    {
        glowCube = transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit() {
        Debug.Log("breakable smacked");
        StartCoroutine(fillWithLight());
    }

    private IEnumerator fillWithLight() {
        
        while(glowCube.localScale.x < 1.99f) {
            glowCube.localScale += new Vector3(1f ,1f ,1f) * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
