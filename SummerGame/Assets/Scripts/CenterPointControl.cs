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

    void Start() {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        StartCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        EndCollider = transform.GetChild(1).GetComponent<SphereCollider>();

        StartCollider.radius = startDistance;
        EndCollider.radius = endDistance;
    }

    public void updateCenterState() {
        controller.findNewCenter();
    }
}