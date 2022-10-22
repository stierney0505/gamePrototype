using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water5Script : MonoBehaviour, spell
{
    float damage;
    char type;
    public Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void createOnHiteffect() { }
    public void remove() { Destroy(gameObject); }
    public void end() { }

    public void relocate()
    {
        Vector3 getPos = transform.position;
        transform.position = new Vector3(getPos.x, getPos.y + 8.14f, getPos.z);
    }

    public float getDamage() { return damage; }
    public char getType() { return type; }
}
