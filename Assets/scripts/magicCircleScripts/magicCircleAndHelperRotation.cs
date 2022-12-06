using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class magicCircleAndHelperRotation : MonoBehaviour
{
    //for them to rotate around a point so they will just have be apart of the parent circle and have faster counter rotations
    //for now.
    public int direction, count; //Should be either -1 or 1 to indicate clockwise or counterclockwise rotation
    public float rotationSpeed; //The rotation speed of the magicCircle
    public Transform location;
    public bool isChild; //This bool just checks if the circle is a child, and if so doesn't need to perform the repositions based on the 'location' transform

    void Start()
    {
        if (direction == 0)
            direction = 1;
        if(!isChild)
            transform.position = location.position;

        for (int i = 1; i < count + 1; i++)
        {
            GameObject circle1 = Instantiate(Resources.Load("MagicCircles/helperCircles/" + getName() + "Helper")) as GameObject;
            figure8Script circle1Script = circle1.GetComponent<figure8Script>();


            circle1Script.count = i;

            circle1Script.parentLoc = transform.position;
            circle1Script.speed = 2;
            circle1Script.parent = gameObject;
            circle1.transform.position = transform.position;
            circle1.transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
        }

    }

    private string getName()
    {
        int indexNum = name.IndexOfAny("123456".ToCharArray());
        return name.Substring(0, indexNum+1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {   if(!isChild)
            transform.position = location.position;
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);
    }
}
