using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water5Script : spell
{
    public override Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public override void end()
    {
        Destroy(gameObject);
    }

    public void relocate()
    {
        Vector3 getPos = transform.position;
        transform.position = new Vector3(getPos.x, getPos.y + 8.14f, getPos.z);
    }
}
