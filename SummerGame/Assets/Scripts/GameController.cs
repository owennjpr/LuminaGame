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
    private bool slowfallActivated;
    private bool slowfallInUse;
    public Image slowfallMask;
    private Color slowfallMaskColor;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        playerVector = new Vector2(player.transform.position.x, player.transform.position.z);
        
        findNewCenter();

        fadeColor = fullScreenFade.color;
        DynamicFadeColor = DynamicFullScreenFade.color;
        DynamicFadeColor.a = 0.0f;
        fadeColor.a = 0.0f;
        
        controller = player.GetComponent<FirstPersonController>();
        slowfallActivated = false;
        slowfallInUse = false;
        slowfallMaskColor = slowfallMask.color;
        slowfallMaskColor.a = 0.0f;
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

    // figures out what center point player in rn so can figure out what should be able to do
    // called at start() but also when enter or leave centers
    public void findNewCenter() {
        GameObject[] centers = GameObject.FindGameObjectsWithTag("centerpoint");
        GameObject[] activeCenters = {null, null};

        // for each center checks if player within range adds to activeCenters array
        // player should never be in more than two centers 
        foreach(GameObject c in centers) {
            Vector2 currCenterVector = new Vector2(c.transform.position.x, c.transform.position.z);
            float distance = Vector2.Distance(currCenterVector, playerVector);
            
            if (distance <= c.GetComponent<CenterPointControl>().startDistance) {
                if(activeCenters[0] == null) {
                    activeCenters[0] = c;
                } else {
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

            // if in two overlapping always puts you in bigger one
            if (c1range > c2range) {
                CenterPoint = activeCenters[0].transform;
            } else {
                CenterPoint = activeCenters[1].transform;
            }
        }

        fadeStartDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().startDistance;
        fadeEndDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().endDistance;
        fadeRange = fadeEndDistance - fadeStartDistance;

        centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);
        movingCenter = CenterPoint.gameObject.GetComponent<CenterPointControl>().isMoving;
        if (!movingCenter) {
            StableCenterPoint = CenterPoint;
        }
        RenderSettings.fogDensity = 1.2f/fadeStartDistance;
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

    public void slowfall() {
        Debug.Log("Slowly falling");
        slowfallActivated = true;
        StartCoroutine(fadeinSlowfall());
    }

    public void playerJumped() 
    {
        Debug.Log("jumped");
        if (slowfallActivated) {
            slowfallInUse = true;
            StartCoroutine(changeGrav());
        }
    }
    
    public void playerLanded() 
    {
        Debug.Log("landed");
        if (slowfallInUse) {
            slowfallActivated = false;
            slowfallInUse = false;

            StartCoroutine(fadeoutSlowfall());
        }
    }

    private IEnumerator fadeinSlowfall() {
        while (slowfallMask.color.a < 0.10f) {
            slowfallMaskColor.a += Time.deltaTime;
            slowfallMask.color = slowfallMaskColor;
            yield return null;
        }
    }

    private IEnumerator fadeoutSlowfall() {
        controller.m_GravityMultiplier = 2;
        while (slowfallMask.color.a > 0) {
            slowfallMaskColor.a -= Time.deltaTime;
            slowfallMask.color = slowfallMaskColor;
            yield return null;
        }
    }

    private IEnumerator changeGrav() {
        yield return new WaitForSeconds(0.5f);
        if (slowfallInUse) {
            controller.m_GravityMultiplier = 0.2f;
        }
    }
}


