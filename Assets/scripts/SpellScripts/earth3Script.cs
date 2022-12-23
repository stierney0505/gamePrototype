using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth3Script : MonoBehaviour, spell
{
    float animSpeed = 1f, speed = 5f;
    private Animator animator;
    public float damage, knockBack;
    public char type;
    Vector3 startLoc, endLoc;
    bool hit = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = animSpeed;
        
        endLoc = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerSource = player.transform.GetChild(0).position;
        startLoc = playerSource;
        transform.position = startLoc;
        rotate();
    }

    private void Update()
    {
        if (transform.position.Equals(endLoc)) { extend(); }
        if(!hit) 
            transform.position = Vector2.MoveTowards(transform.position, endLoc, Time.deltaTime * speed); 
    }

    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;
    }

    public void remove() { Destroy(gameObject); }
    public void end(bool environment)
    {  
        hit = true;
        animator.SetTrigger("hit");
        animator.speed = 1.25f;
        speed = 0;
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

    public void incrementSpeed()
    {
        speed += speed/5.75f;
        damage += damage /4f;
        knockBack += knockBack / 4.5f;
        if (speed >= 16 && !hit)
        {
            animator.SetTrigger("maxSpeed");
            animator.speed = 1.45f;
            damage = 60;
        }
    }

    public float getDamage() { return damage; }
    public char getType() { return type; }
    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
