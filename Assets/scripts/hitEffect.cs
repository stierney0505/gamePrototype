using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0,181);
        var rotation = Quaternion.AngleAxis((float)rand, Vector3.forward);
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void die() { Destroy(gameObject); }
}
