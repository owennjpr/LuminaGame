using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(selfDestruct());
    }

    private IEnumerator selfDestruct() {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
