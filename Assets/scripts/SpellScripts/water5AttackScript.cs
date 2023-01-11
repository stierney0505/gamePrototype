using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water5AttackScript : MonoBehaviour, spell
{
    public float damage, knockBack, rotation;
    public dLRSNode.types type;
    public Transform parent;
    public bool left;
    Animator animator;
    Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        transform.position = parent.position;
        transform.Rotate(0, 0, rotation);

        if (left)
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;
    }
    public void remove() { Destroy(gameObject); }
    public void end(bool environment)
    {
        if (environment)
            remove();
        animator.SetTrigger("fade"); }
    public float getDamage() { return damage; }
    public dLRSNode.types getType() { return type; }
    public void enableCollider() { col.enabled = true; }
    public void disableCollider() { col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
