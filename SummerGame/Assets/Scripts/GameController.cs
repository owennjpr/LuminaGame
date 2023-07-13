using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class GameController : MonoBehaviour
{
    public Transform CenterPoint;
    public Transform StableCenterPoint;

    public GameObject player;
    private Vector2 playerVector;
    private Vector2 centerVector;
    public Image fullScreenFade;
    public Image DynamicFullScreenFade;
    private Color fadeColor;
    private Color DynamicFadeColor;
    public float fadeStartDistance;
    public float fadeEndDistance;
    private float fadeRange;
    private bool movingCenter;

    public FirstPersonController controller;
    // Start is called before the first frame update
    void Start()
    {
        findNewCenter();

        player = GameObject.FindWithTag("Player");

        fadeColor = fullScreenFade.color;
        DynamicFadeColor = DynamicFullScreenFade.color;
        DynamicFadeColor.a = 0.0f;
        fadeColor.a = 0.0f;
        
        controller = player.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingCenter) {
            centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);
        }
        playerVector = new Vector2(player.transform.position.x, player.transform.position.z);
        
        float distance = Vector2.Distance(centerVector, playerVector);
        // Debug.Log(distance);
        if (distance >= fadeStartDistance) {
            float x = (((distance - fadeStartDistance)/fadeRange));
            // Debug.Log(x);
            fadeColor.a = Mathf.Min(x, 1.0f);
            if (x > 1.5f) {
                StartCoroutine(oobReturn());
            }
        } else {
            fadeColor.a = 0.0f;
        }
        fullScreenFade.color = fadeColor;
    }

    private IEnumerator oobReturn() {
        controller.haltWalk = true;
        if (CenterPoint == StableCenterPoint) {
            controller.flipped = !(controller.flipped);
            player.transform.position = Vector3.MoveTowards(player.transform.position, CenterPoint.position, 5);
        } else {
            Vector3 stablePos = StableCenterPoint.position;
            player.transform.position = stablePos + StableCenterPoint.GetComponent<CenterPointControl>().relativeSpawnPos;
        }
        
        yield return new WaitForSeconds(0.05f);
        controller.haltWalk = false;
    }

    public void findNewCenter() {
        // Debug.Log("updating");
        GameObject[] centers = GameObject.FindGameObjectsWithTag("centerpoint");
        GameObject[] activeCenters = {null, null};
        foreach(GameObject c in centers) {
            Vector2 currCenterVector = new Vector2(c.transform.position.x, c.transform.position.z);
            float distance = Vector2.Distance(currCenterVector, playerVector);
            // Debug.Log(distance);

            if (distance <= c.GetComponent<CenterPointControl>().startDistance) {
                // Debug.Log("found overlapping");
                if(activeCenters[0] == null) {
                    // Debug.Log("found center 1");
                    activeCenters[0] = c;
                } else {
                    // Debug.Log("found center 2");
                    activeCenters[1] = c;
                }
            }
        }

        if (activeCenters[0] == null) {
            return;
        } else if (activeCenters[1] == null) {
            CenterPoint = activeCenters[0].transform;
        } else {
            float c1range = activeCenters[0].GetComponent<CenterPointControl>().startDistance;
            float c2range = activeCenters[1].GetComponent<CenterPointControl>().startDistance;
            if (c1range > c2range) {
                CenterPoint = activeCenters[0].transform;
            } else {
                CenterPoint = activeCenters[1].transform;
            }
        }

        fadeStartDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().startDistance;
        fadeEndDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().endDistance;
        fadeRange = fadeEndDistance - fadeStartDistance;

        // Debug.Log(fadeStartDistance);
        // Debug.Log(fadeEndDistance);
        centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);
        movingCenter = CenterPoint.gameObject.GetComponent<CenterPointControl>().isMoving;
        if (!movingCenter) {
            StableCenterPoint = CenterPoint;
        }
    }
    
    public void checkFade() {
        if (fadeColor.a > 0.0f) {
            StartCoroutine(manualFade(fadeColor.a));
            Debug.Log("Howdy");
        }
    }

    private IEnumerator manualFade(float startingAlpha) {
        float currAlpha = startingAlpha;
        while (currAlpha > 0.0f) {
            // Debug.Log(currAlpha);
            DynamicFadeColor.a = currAlpha;
            DynamicFullScreenFade.color = DynamicFadeColor;
            currAlpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        DynamicFadeColor.a = 0.0f;
    }
}


