using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning5Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    int loops = 0;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        int rand = Random.Range(0, 180);
        transform.eulerAngles = new Vector3(0, 0, rand);
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
    }
    public float getDamage() { return damage; }
     public dLRSNode.types getType() { return type; }

    public void enableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = true; }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void checkLoops()
    {
        int rand = Random.Range(0, 180);
        transform.eulerAngles = new Vector3(0, 0, rand);
        if (loops < 4)
        {
            loops++;
            enableCollider();
            damage *= 4;
        }
        else
            animator.SetTrigger("fade");
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
