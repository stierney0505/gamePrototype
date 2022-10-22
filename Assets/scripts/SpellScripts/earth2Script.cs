using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth2Script : MonoBehaviour, spell
{
    int loops = 0;
    float damage;
    char type;
    public Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public void remove() { Destroy(gameObject); }
    public void end() { }

    public void createOnHiteffect() { }

    public void loop()
    {
        if(loops < 10) { loops++; }
        else { remove(); }
    }

    public float getDamage() { return damage; }
    public char getType() { return type; }
}
