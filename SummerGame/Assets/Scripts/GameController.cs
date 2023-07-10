using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Transform CenterPoint;
    private GameObject player;
    private Vector2 playerVector;
    private Vector2 centerVector;
    public Image fullScreenFade;
    private Color fadeColor;
    public float fadeStartDistance;
    public float fadeEndDistance;
    private float fadeRange;
    // Start is called before the first frame update
    void Start()
    {
        CenterPoint = transform.GetChild(0).transform;
        centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);

        player = GameObject.FindWithTag("Player");

        fadeColor = fullScreenFade.color;
        fadeColor.a = 0.0f;
        
        fadeRange = fadeEndDistance - fadeStartDistance;
        // if (CenterPoint != null & player != null)
        // {
        //     Debug.Log("YAYAYAYAYAYAYAY");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        playerVector = new Vector2(player.transform.position.x, player.transform.position.z);
        
        float distance = Vector2.Distance(centerVector, playerVector);
        //Debug.Log(distance);
        
        if (distance >= fadeStartDistance) {
            float x = (((distance - fadeStartDistance)/fadeRange));
            // Debug.Log(x);
            fadeColor.a = Mathf.Min(x, 1.0f);
        } else {
            fadeColor.a = 0.0f;
        }
        fullScreenFade.color = fadeColor;

    }
}
