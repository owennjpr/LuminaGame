using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLightFloat : MonoBehaviour
{
    
    private Vector3 startPos;
    private float time;
    private float idle_xMod;
    private float idle_yMod;
    private float idle_zMod;
    [SerializeField] private int ID;

    public Material mat1;
    public Material mat2;
    public Material mat3;
    public Material mat4;
    // Start is called before the first frame update
    void Start()
    {
        idle_xMod = Random.Range(1f, 2f);
        idle_yMod = Random.Range(1f, 2f);
        idle_zMod = Random.Range(1f, 2f);
        time = Random.Range(0f, 10f);

        startPos = transform.position;

        switch (ID) {
            case 0:
            transform.GetComponent<MeshRenderer> ().material = mat1;
                break;
            case 1:
            transform.GetComponent<MeshRenderer> ().material = mat2;
                break;
            case 2:
            transform.GetComponent<MeshRenderer> ().material = mat3;
                break;
            case 3:
            transform.GetComponent<MeshRenderer> ().material = mat4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        float y = Mathf.Sin(time / idle_yMod);
        float x = Mathf.Sin(time / idle_xMod);
        float z = Mathf.Sin (time / idle_zMod);
        transform.position = startPos + new Vector3(0.3f * x, 0.5f * y, 0.2f * z);
    }
}
