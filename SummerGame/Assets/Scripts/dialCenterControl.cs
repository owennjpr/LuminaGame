using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialCenterControl : MonoBehaviour
{
    private DialController dial;
    
    // Start is called before the first frame update
    void Start()
    {
        dial = transform.parent.GetComponent<DialController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clicked() {
        dial.centerClicked();
    }
}
