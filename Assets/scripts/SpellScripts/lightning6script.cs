using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning6script : spell
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
}
