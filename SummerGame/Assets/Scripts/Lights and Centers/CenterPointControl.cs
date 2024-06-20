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
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject lights;
    [SerializeField] private GameObject centerObjects;
    private Transform player;

    void Start() {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        StartCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        EndCollider = transform.GetChild(1).GetComponent<SphereCollider>();

        StartCollider.radius = startDistance - 1;
        EndCollider.radius = startDistance;
        player = GameObject.FindWithTag("Player").transform;

        if (!isMoving) {
            particles = transform.GetChild(2).gameObject;
            lights = transform.GetChild(3).gameObject;
            centerObjects = transform.GetChild(4).gameObject;
            deactivate();
        }
    }

    // void Update() {
    //     if (!isMoving && particles != null && lights != null && centerObjects != null) {
    //         if (Vector3.Distance(transform.position, player.position) > (endDistance + 10)) {
    //             particles.SetActive(false);
    //             lights.SetActive(false);
    //             centerObjects.SetActive(false);
    //         } else {
    //             particles.SetActive(true);
    //             lights.SetActive(true);
    //             centerObjects.SetActive(true);
    //         }
    //     }
    // }

    public void updateCenterState() {
        controller.tryFindNewCenter();
    }

    public void checkFade() {
        controller.checkFade();
    }
    
    public void activate() {
        if (!isMoving && particles != null && lights != null && centerObjects != null) {
            particles.SetActive(true);
            lights.SetActive(true);
            centerObjects.SetActive(true);
        }
    }

    public void deactivate() {
        if (!isMoving && particles != null && lights != null && centerObjects != null) {
            particles.SetActive(false);
            lights.SetActive(false);
            centerObjects.SetActive(false);
        }
    }
}