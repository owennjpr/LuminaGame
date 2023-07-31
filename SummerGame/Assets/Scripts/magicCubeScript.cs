using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class magicCubeScript : MonoBehaviour
{
    [SerializeField] private Material activeMat;
    private Transform particleEffect;
    private bool animationStarted;
    [SerializeField] private FirstPersonController playerControl;
    private GameController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        particleEffect = transform.GetChild(0);
        particleEffect.GetChild(0).GetComponent<ParticleSystem>().Stop();
        particleEffect.GetChild(1).GetComponent<ParticleSystem>().Stop();
        controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        
        animationStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 1, 0) * Time.deltaTime * 25;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !animationStarted) {
            Debug.Log("Collided");
            animationStarted = true;
            StartCoroutine(activateCube());
        }
    }

    private IEnumerator activateCube() {
        particleEffect.GetChild(0).GetComponent<ParticleSystem>().Play();
        particleEffect.GetChild(1).GetComponent<ParticleSystem>().Play();

        Vector3 particlePosition = particleEffect.localPosition;
        Debug.Log(particlePosition.y);
        while (particlePosition.y < 0) {
            particlePosition += new Vector3(0, Time.deltaTime, 0);
            particleEffect.localPosition = particlePosition;
            yield return null;
        }
        
        var main = particleEffect.GetChild(1).GetComponent<ParticleSystem>().main;
        main.simulationSpeed = 0.4f;
        particleEffect.GetChild(1).GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1.5f);
        GetComponent<MeshRenderer>().material = activeMat;
        yield return new WaitForSeconds(0.5f);

        particleEffect.GetChild(0).GetComponent<ParticleSystem>().Stop();

        yield return new WaitForSeconds(5);
        BoxCollider volumeCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        Vector3 volumeSize = volumeCollider.size;
        playerControl.m_WalkSpeed = 0;
        playerControl.m_RunSpeed = 0;
        while(volumeSize.x < 100) {
            volumeSize += new Vector3(1, 1, 1) * Time.deltaTime * 15;
            volumeCollider.size = volumeSize;
            yield return null;
        }
        controller.newPowerFound();
        yield return new WaitForSeconds(3);

        while(volumeSize.x > 1) {
            volumeSize -= new Vector3(1, 1, 1) * Time.deltaTime * 50;
            volumeCollider.size = volumeSize;
            yield return null;
        }
        playerControl.m_WalkSpeed = 5;
        playerControl.m_RunSpeed = 10;
        Vector3 myscale = transform.localScale;
        while(myscale.x > 0.001) {
            myscale -= new Vector3(1, 1, 1) * Time.deltaTime /1.5f;
            transform.localScale = myscale;
            yield return null;
        }

        Destroy(gameObject);

    }
}
