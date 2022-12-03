using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicCircleRotationScript : MonoBehaviour
{
    public int direction; //Should be either -1 or 1 to indicate clockwise or counterclockwise rotation
    public float rotationSpeed; //The rotation speed of the magicCircle
    public Transform location;

    void Start()
    {
        transform.position = location.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   transform.position = location.position;
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime); 
    }
}
