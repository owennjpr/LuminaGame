using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupTrigger : MonoBehaviour
{
    
    public popupText popup;
    private bool hit;
    [Header("Popup 1")]
    public bool isTitle;
    public string mainText;
    public string subText;
    public float secondsActive;

    [Header("Popup 2")]
    public bool consecutive;
    public float gapTime;
    public bool isTitle2;
    public string mainText2;
    public string subText2;
    public float secondsActive2;

    // Start is called before the first frame update
    void Start()
    {
        hit = false;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player" && !hit) {
            hit = true;
            StartCoroutine(handleTrigger());
        }
    }

    private IEnumerator handleTrigger() {
        // display the first popup
        if (isTitle) {
            StartCoroutine(popup.CenterPopupAppear(mainText, subText, secondsActive));
        } else {
            StartCoroutine(popup.LowerPopupAppear(mainText, secondsActive));
        }
        yield return new WaitForSeconds(secondsActive + gapTime);
        // display the second
        if (consecutive) {
            if (isTitle2) {
                StartCoroutine(popup.CenterPopupAppear(mainText2, subText2, secondsActive2));
            } else {
                StartCoroutine(popup.LowerPopupAppear(mainText2, secondsActive2));
            }
            yield return new WaitForSeconds(secondsActive2);
        } else {
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
