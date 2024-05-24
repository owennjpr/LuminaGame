using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSelfDestruct : MonoBehaviour
{
    // private Color glowYellow;
    // private Color glowBlue;
    // private Color glowPurple;
    // private Color glowRed;
    private ParticleSystem pSystem;
    // Start is called before the first frame update
    void Awake()
    {
        // glowYellow = new Color(255, 230, 71);
        // glowBlue = new Color(46, 238, 255);
        // glowPurple = new Color(162, 90, 255);
        // glowRed = new Color(255, 44, 42);
        pSystem = gameObject.GetComponent<ParticleSystem>();
        // pSystem.Stop();
        StartCoroutine(selfDestruct());
    }

    public void setColor(int ID) {
        pSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
// InitialModule.startColor
        ParticleSystem.MainModule pMain = pSystem.main;
        Debug.Log("Color ID: " + ID);
        switch (ID) {
            case 0:
                pMain.startColor = new Color(1f, 0.92f, 0.35f);
                break;
            case 1:
                pMain.startColor = new Color(0.24f, 0.99f, 1f);
                break;
            case 2:
                pMain.startColor = new Color(0.7f, 0.41f, 1f);
                break;
            case 3:
                pMain.startColor = new Color(1f, 0.17f, 0.17f);
                break;
        }
        Debug.Log("main mod start color: " + pSystem.main.startColor);
        pSystem.Clear();
        pSystem.Play();
    }

    private IEnumerator selfDestruct() {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
