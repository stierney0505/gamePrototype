using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning6script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack;
    [SerializeField] char type; 
    public Vector3 getVector()
    {
        
        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void remove() { Destroy(gameObject); }
    public void end() { }
    public float getDamage() { return damage; }
    public char getType() { return type; }

    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
