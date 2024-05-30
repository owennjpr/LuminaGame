using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLightReturn : MonoBehaviour
{
    private Vector3 startPos;
    private bool beingPulled;
    private Transform playerHand;
    private Transform controller;
    private bool idle;
    private float time;
    private float idle_xMod;
    private float idle_yMod;
    private float idle_zMod;
    private bool clicked;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeIn());
        idle_xMod = Random.Range(0.5f, 2.5f);
        idle_yMod = Random.Range(0.5f, 2.5f);
        idle_zMod = Random.Range(0.5f, 2.5f);
        idle = true;
        time = 0;
        controller = GameObject.FindWithTag("GameController").transform;
        startPos = transform.position;
        playerHand = GameObject.FindWithTag("MainCamera").transform.GetChild(0);
        clicked = false;
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1)) {
            beingPulled = false;
        }
        if (beingPulled) {
            idle = false;
            if (transform.position != playerHand.position) {
                transform.position = Vector3.MoveTowards(transform.position, playerHand.position, 15*Time.deltaTime);
            } 
            if (Vector3.Distance(transform.position, playerHand.position) < 0.25f && !clicked) {
                if (Input.GetKeyDown("e") || Input.GetMouseButtonDown(0) || Input.GetKeyDown("q")) {
                    Debug.Log("CLICKED");
                    clicked = true;
                    StartCoroutine(controller.GetComponent<GameController>().manualFadeIN());
                }
            }
        } else {
            if (transform.position != startPos) {
                transform.position = Vector3.MoveTowards(transform.position, startPos, 3*Time.deltaTime);
            } else {
                idle = true;
                time = 0;
            }
        }
        if (idle) {
            time += Time.deltaTime;
            float y = Mathf.Sin(time / idle_yMod);
            float x = Mathf.Sin(time / idle_xMod);
            float z = Mathf.Sin (time / idle_zMod);
            transform.position = startPos + new Vector3(0.4f * x, 0.6f * y, 0.4f * z);
        }
    }

    public void playerClicked() {
        beingPulled = true;
    }

    private IEnumerator fadeIn() {
        transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        Vector3 myscale = transform.localScale;
        while(myscale.x < 1f) {
            myscale += new Vector3(1, 1, 1) * Time.deltaTime/2;
            transform.localScale = myscale;
            yield return null;
        }
    }

}
