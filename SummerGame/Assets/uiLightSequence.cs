using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class uiLightSequence : MonoBehaviour
{
    
    public GameObject uiElemObj;
    private List<int> lightSequence;
    private Color yellowColor;
    private Color blueColor;
    private Color purpleColor;
    private Color redColor;

    // Start is called before the first frame update
    void Start()
    {
        lightSequence = new List<int>();
        yellowColor = new Color(1f, 0.95f, 0.7f);
        blueColor = new Color(0.7f, 1f, 1f);
        purpleColor = new Color(0.9f, 0.7f, 1f);
        redColor = new Color(1f, 0.7f, 0.7f);
    }

    public void clear() {
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
        // lightSequence.Clear();
    }

    public void updateSequence(int followColor) {
        // Debug.Log(followColor);
        // Debug.Log(lightSequence[0]);
        
        if(lightSequence.Count > 0) {
            if (followColor == lightSequence[0]) {
                lightSequence.RemoveAt(0);
                newSequence(lightSequence);
            } else {
                clear();
                lightSequence.Clear();
            }
        }
        
    }
    public void newSequence (List<int> seq) {
        // List<int> temp = seq;
        clear();
        lightSequence = seq;
        Debug.Log(seq.Count);
        for (int i = 0; i < lightSequence.Count; i++) {
            // Debug.Log(lightSequence[i]);
            int offset = i * 60;
            GameObject elem = Instantiate(uiElemObj, transform.position + new Vector3(offset, 0, 0), Quaternion.identity, transform);
            Image img = elem.GetComponent<Image>();
            switch (lightSequence[i]) {
                case 0:
                    img.color = yellowColor;
                    break;
                case 1:
                    img.color = blueColor;
                    break;
                case 2:
                    img.color = purpleColor;
                    break;
                case 3:
                    img.color = redColor;
                    break;
            }
        } 
    }
}
