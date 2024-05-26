using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialSymbolHitCheck : MonoBehaviour
{
    private GameController gameControl;
    public int directionID;
    
    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") {
            gameControl.AddNewDialToUI(directionID);
            Destroy(gameObject);
        }
    }
}
