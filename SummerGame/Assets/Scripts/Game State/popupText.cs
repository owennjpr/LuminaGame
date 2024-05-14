using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class popupText : MonoBehaviour
{
    [SerializeField] private GameObject centerText;
    [SerializeField] private GameObject centerSubText;
    [SerializeField] private GameObject lowerTextBox;

    [SerializeField] private TMP_Text centerTMP;
    [SerializeField] private TMP_Text centerSubTMP;

    [SerializeField] private TMP_Text lowerTMP;


    // Start is called before the first frame update
    void Start()
    {
        centerText = transform.GetChild(0).gameObject;
        centerSubText = centerText.transform.GetChild(0).gameObject;
        centerTMP = centerText.GetComponent<TMP_Text>();
        centerSubTMP = centerSubText.GetComponent<TMP_Text>();
        centerText.SetActive(false);

        lowerTextBox = transform.GetChild(1).gameObject;
        lowerTMP = lowerTextBox.transform.GetChild(0).GetComponent<TMP_Text>();
        lowerTextBox.SetActive(false);
        StartCoroutine(LowerPopupAppear("Howdy", 3.2f));
        StartCoroutine(CenterPopupAppear("NEW NEW NEW", "Greetings there player", 2));
    }

    public IEnumerator LowerPopupAppear(string text, float secondsActive) {
        lowerTMP.text = text;
        lowerTextBox.SetActive(true);
        Image backgroundImage = lowerTextBox.GetComponent<Image>();

        Color textColor = new Color(1, 1, 1, 0);
        Color backgroundColor = backgroundImage.color;
        lowerTMP.color = textColor;
        for(int i = 0; i < 255; i++) {
            textColor.a = i/255f;
            backgroundColor.a = i/400f;
            lowerTMP.color = textColor;
            backgroundImage.color = backgroundColor;
            yield return new WaitForSeconds(Time.deltaTime / 7);
        }
        yield return new WaitForSeconds(secondsActive);
        for(int i = 255; i > 0; i--) {
            textColor.a = i/255f;
            backgroundColor.a = i/400f;
            lowerTMP.color = textColor;
            backgroundImage.color = backgroundColor;
            yield return new WaitForSeconds(Time.deltaTime / 7);
        }
        lowerTextBox.SetActive(false);
    }

    public IEnumerator CenterPopupAppear(string mainText, string subText, float secondsActive) {
        centerTMP.text = mainText;
        centerSubTMP.text = subText;
        centerText.SetActive(true);
        centerSubText.SetActive(true);
        yield return new WaitForSeconds(secondsActive);
        centerText.SetActive(false);
        centerSubText.SetActive(false);

    }
}
