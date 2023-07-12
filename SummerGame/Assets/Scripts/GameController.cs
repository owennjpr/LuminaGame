using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class GameController : MonoBehaviour
{
    private Transform CenterPoint;
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
        CenterPoint = transform.GetChild(0).transform;
        centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);

        player = GameObject.FindWithTag("Player");

        fadeColor = fullScreenFade.color;
        fadeColor.a = 0.0f;
        
        fadeRange = fadeEndDistance - fadeStartDistance;

        controller = player.GetComponent<FirstPersonController>();
        // if (CenterPoint != null & player != null)
        // {
        //     Debug.Log("YAYAYAYAYAYAYAY");
        // }
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
    
}


