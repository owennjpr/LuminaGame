using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class GameController : MonoBehaviour
{
    public Transform CenterPoint;
    public GameObject player;
    private Vector2 playerVector;
    private Vector2 centerVector;
    public Image fullScreenFade;
    private Color fadeColor;
    public float fadeStartDistance;
    public float fadeEndDistance;
    private float fadeRange;

    public FirstPersonController controller;
    // Start is called before the first frame update
    void Start()
    {
        findNewCenter();

        centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);

        player = GameObject.FindWithTag("Player");

        fadeColor = fullScreenFade.color;
        fadeColor.a = 0.0f;
        
        fadeRange = fadeEndDistance - fadeStartDistance;

        controller = player.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!returning) {
            playerVector = new Vector2(player.transform.position.x, player.transform.position.z);
        
            float distance = Vector2.Distance(centerVector, playerVector);
        
            if (distance >= fadeStartDistance) {
                float x = (((distance - fadeStartDistance)/fadeRange));
                fadeColor.a = Mathf.Min(x, 1.0f);
                if (x > 1.5f) {
                    // controller.testWalk = false;
                    // controller.flipped = !(controller.flipped);
                    // player.transform.position = Vector3.MoveTowards(player.transform.position, CenterPoint.position, 5);
                    // controller.testWalk = true;
                    // fadeColor.a = 0.0f;
                    // fullScreenFade.color = fadeColor;
                    StartCoroutine(oobReturn());
                    
                }
            } else {
                fadeColor.a = 0.0f;
            }
            fullScreenFade.color = fadeColor;
    }

    private IEnumerator oobReturn() {
        controller.haltWalk = true;
        controller.flipped = !(controller.flipped);
        player.transform.position = Vector3.MoveTowards(player.transform.position, CenterPoint.position, 5);
        yield return new WaitForSeconds(0.05f);
        controller.haltWalk = false;
    }

    public void findNewCenter() {
        Debug.Log("updating");
        GameObject[] centers = GameObject.FindGameObjectsWithTag("centerpoint");
        GameObject[] activeCenters = {null, null};
        foreach(GameObject c in centers) {
            Vector2 currCenterVector = new Vector2(c.transform.position.x, c.transform.position.z);
            float distance = Vector2.Distance(currCenterVector, playerVector);
            // Debug.Log("found center");

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

    }
    
}


