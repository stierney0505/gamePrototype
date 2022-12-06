using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicCircleRotationScript : MonoBehaviour //This script is pretty basic, just takes a transform to keep the spell circle in a position and 
{                                                      //takes either 1 or -1 for rotation. Most spell circles have childrne and I have not added the functionality 
                                                       //for them to rotate around a point so they will just have be apart of the parent circle and have faster counter rotations
                                                       //for now.
    public int direction; //Should be either -1 or 1 to indicate clockwise or counterclockwise rotation
    public float rotationSpeed; //The rotation speed of the magicCircle
    public Transform location;
    public bool isChild; //This bool just checks if the circle is a child, and if so doesn't need to perform the repositions based on the 'location' transform

    void Start()
    {
        if (direction == 0)
            direction = 1;

        if(!isChild)
            transform.position = location.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   if(!isChild)
            transform.position = location.position;
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime); 
    }
}
