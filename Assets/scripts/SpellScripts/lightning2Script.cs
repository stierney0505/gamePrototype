using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning2Script : MonoBehaviour, spell
{
    float damage, knockBack;
    char type;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public void remove() { Destroy(gameObject); }
    public void end() { animator.speed = 1; }
    public void setSpeedZero() { animator.speed = 0; }
    public float getDamage() { return damage; }
    public char getType() { return type; }
    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }
}
