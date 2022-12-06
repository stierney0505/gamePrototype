using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class figure8Script : MonoBehaviour
{
    public int direction, count; //Should be either -1 or 1 to indicate clockwise or counterclockwise rotation
    public float rotationSpeed, speed;
    public Vector2 parentLoc;
    public GameObject parent;
    Vector2 dir;
    float time = 0, x, y, scale; //these hold the damage values of damage and knockBack so that they can be activated after the movement portion of the spell

    // Start is called before the first frame update
    void Start()
    {
        if (direction == 0)
            direction = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(parent == null)
            Destroy(gameObject);
        time += Time.deltaTime;
        scale = 2 / (3 - Mathf.Cos(2 * time));
        int dist = 6;

        switch (count)
        {
            case 1:
                x = 6 * scale * Mathf.Cos(time);
                y = 6 * scale * Mathf.Sin(2 * time) / 2;
                break;
            case 2:
                x = -6 * scale * Mathf.Cos(time);
                y = -6 * scale * Mathf.Sin(2 * time) / 2;
                break;
            case 3:
                x = 6 * scale * Mathf.Cos(time);
                y = -6 * scale * Mathf.Sin(2 * time) / 2;
                break;
            case 4:
                x = -6 * scale * Mathf.Cos(time);
                y = 6 * scale * Mathf.Sin(2 * time) / 2;
                break;
            default:
                break;
        }
            
      
        dir = new Vector2(parentLoc.x + x, parentLoc.y + y) - (Vector2)transform.position;
        transform.Translate(dir * Time.deltaTime * speed, Space.World);
        transform.Rotate(0, 0, direction * rotationSpeed * Time.deltaTime);

    }
}
