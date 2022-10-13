using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth2Script : spell
{
    int loops = 0;
    public override Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public override void end()
    {
        Destroy(gameObject);
    }

    public void loop()
    {
        if(loops < 10) { loops++; }
        else { end(); }
    }
}
