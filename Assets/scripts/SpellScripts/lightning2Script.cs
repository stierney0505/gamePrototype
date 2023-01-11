using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class lightning2Script : MonoBehaviour, spell
{
    public float damage, knockBack, speed;
    public dLRSNode.types type;
    Animator animator;
    bool projectile = false;
    Vector2 endLoc, startLoc;
    float projectileLife = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        startLoc = player.transform.GetChild(0).position;
        rotate(startLoc, transform.position);
        endLoc = transform.position;
        transform.position = startLoc;
    }
    private void Update() //make fireball keep traveling in the direction
    {
        if (projectile)
        {
            if (transform.position.Equals(endLoc)) { extend(); }
            projectileLife += Time.deltaTime;
            if (projectileLife >= 2)
                end(false);
            else
                transform.position = Vector2.MoveTowards(transform.position, endLoc, Time.deltaTime * speed);
        }
    }
    public void setAnimSpeedZero()
    {
        animator.speed = 0;
        transform.localScale = new Vector2(4, 4);
        projectile = true;
        gameObject.transform.rotation = Quaternion.identity;
        rotate();
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
        animator.SetTrigger("fade"); 
        animator.speed = 1; 
        speed = 0; }
    public float getDamage() { return damage; }
     public dLRSNode.types getType() { return type; }
    public void rotate(Vector2 start, Vector2 end)
    {
        if (start.x > end.x) { transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); }
        float startX = start.x;
        float startY = start.y;
        float endX = end.x;
        float endY = end.y;
        float slope = (endY - startY) / (endX - startX);
        float rotation = Mathf.Rad2Deg * Mathf.Atan(slope);
        transform.Rotate(0, 0, rotation);
    }

    public void enableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = true; }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void extend()
    {
        float startX = startLoc.x;
        float startY = startLoc.y;
        float endX = endLoc.x;
        float endY = endLoc.y;

        float rise = endY - startY;
        float run = endX - startX;
        endLoc = new Vector2(endLoc.x + (run * 3.0f), startLoc.y + (rise * 3.0f));

    }

    public void rotate()
    {
        float startX = startLoc.x;
        float startY = startLoc.y;
        float endX = endLoc.x;
        float endY = endLoc.y;


        float slope = (endY - startY) / (endX - startX);
        float rotation = Mathf.Rad2Deg * Mathf.Atan(slope);
        transform.Rotate(0, 0, rotation);

        if (endX < startX) { transform.localScale = new Vector2(transform.localScale.x * -1.0f, transform.localScale.y); }
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
