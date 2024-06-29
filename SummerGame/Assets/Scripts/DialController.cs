using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialController : MonoBehaviour
{
    
    private GameController controller;
    public GameObject centerCastObj;
    public GameObject solveTrigger;
    private struct pointData {
        public int[] positions;
        public int[] colors;
    };

    public GameObject knobObject;
    public GameObject pointObject;
    public float distFromCenter;
    [SerializeField] private float stepsize;
    private int knobMask;
    [SerializeField] private int[] correctKnobValues;
    private int correctKnobMask;
    [SerializeField] private int[] colorSequence;
    private bool solved;

    //audio
    private AudioSource audio_s;
    public AudioClip incorrectSFX;
    public AudioClip correctSFX;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        solved = false;
        knobMask = 0;
        correctKnobMask = 0;
        foreach (int n in correctKnobValues) {
            int tempMask = 1 << n;
            correctKnobMask += tempMask;
        }
        audio_s = GetComponent<AudioSource>();
    }


    public void updateMask(bool activate, int knobID) {
        
        int tempMask = 1 << knobID;
        if (activate) {
            knobMask += tempMask;
        } else {
            knobMask -= tempMask;
        }
    }

    private void handleSolved() {
        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("dial knob")) {
                // Debug.Log(child.GetComponent<knobControl>().ID);
                int tempMask = correctKnobMask & (1 << child.GetComponent<knobControl>().ID);
                if (tempMask != 0) {
                    child.GetComponent<knobControl>().triggerSolve();
                }
            }
        }
    }

    public void centerClicked() {
        // Debug.Log("You clicked the center!");
        GameObject sphere = Instantiate(centerCastObj, transform.GetChild(0).position, Quaternion.identity, transform);
        if (correctKnobMask == knobMask && controller.hasFoundDials()) {
                audio_s.clip = correctSFX;
                audio_s.Play();
                if (!solved) {
                    solved = true;
                    // Debug.Log("Make a purple light");
                    if (solveTrigger != null) {
                        solveTrigger.SetActive(true);
                    }
                }        
        } else {
            audio_s.clip = incorrectSFX;
            audio_s.Play();
        }
    }
}
