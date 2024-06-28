using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLamp : MonoBehaviour
{
    private Transform glowCube;
    private bool lit;
    public float timer;

    private AudioSource audio_s;

    public TimerResponse objToActivate;
    // Start is called before the first frame update
    void Start()
    {
        glowCube = transform.parent.GetChild(1);
        glowCube.gameObject.SetActive(false);
        lit = false;
        audio_s = gameObject.GetComponent<AudioSource>();
        
    }

    public void hit() {
        Debug.Log("lamp smacked");
        if (!lit) {
            StartCoroutine(fillWithLight());
        }
    }
    
    public IEnumerator fillWithLight() {
        if (!lit) {
            
            glowCube.gameObject.SetActive(true);
            while(glowCube.localScale.x < 0.85f) {
                glowCube.localScale += new Vector3(1f, 1f, 0.5f) * 3f * Time.deltaTime;
                yield return null;
            }
            lit = true;
            audio_s.Play();
            objToActivate.activate();
            while(glowCube.localScale.x > 0.01f) {
                glowCube.localScale -= new Vector3(1f, 1f, 0.5f) * (1 / timer) * Time.deltaTime;
                yield return null;
            }
            glowCube.gameObject.SetActive(false);
            lit = false;
            audio_s.Stop();
            objToActivate.deactivate();
        }
        
    }
}
