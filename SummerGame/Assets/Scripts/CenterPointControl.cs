using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPointControl : MonoBehaviour
{
    public float startDistance;
    public float endDistance;
    private SphereCollider StartCollider;
    private SphereCollider EndCollider;
    public GameController controller;
    public bool isMoving;
    public Vector3 relativeSpawnPos;
    public GameObject particles;
    public GameObject lights;
    private Transform player;

    void Start() {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        StartCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        EndCollider = transform.GetChild(1).GetComponent<SphereCollider>();

        StartCollider.radius = startDistance - 1;
        EndCollider.radius = startDistance;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        if (!isMoving && particles != null && lights != null) {
            if (Vector3.Distance(transform.position, player.position) > (endDistance + 10)) {
                particles.SetActive(false);
                lights.SetActive(false);
            } else {
                particles.SetActive(true);
                lights.SetActive(true);
            }
        }
    }

    public void updateCenterState() {
        controller.findNewCenter();
    }

    public void checkFade() {
        controller.checkFade();
    }
    
}