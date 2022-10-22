using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth3Script : MonoBehaviour, spell
{
    float speed = 0.15f;
    private Animator animator;
    int loops = 0;
    float damage;
    char type;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public void createOnHiteffect() { }

    public void remove() { Destroy(gameObject); }
    public void end() { }

    public void startIncrease()
    {
        if (speed < .75) { speed += speed/3.5f; animator.speed = speed; }
        else { animator.SetTrigger("windUp"); }
    }
    public void windUpIncrease()
    {
        if (speed < 2) { speed += speed/1.5f; animator.speed = speed; }
        else { animator.SetTrigger("maxSpeed"); }
    }
    public void loop()
    {
        if (loops <= 30) { loops++; }
        else { remove(); };
    }

    public float getDamage() { return damage; }
    public char getType() { return type; }
}
