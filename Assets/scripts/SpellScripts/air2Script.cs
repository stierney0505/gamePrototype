using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air2Script : MonoBehaviour, spell //This spell has two parts, it starts by moving the enemies towards its 'center' and then pushes them out dealing damage
{
    [SerializeField] float damage, knockBack, speed; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    internal Collider2D col;
    float tempDamage, tempKnockBack; //these hold the damage values of damage and knockBack so that they can be activated after the movement portion of the spell
    List<Transform> enemies = new List<Transform>();
    bool active = true;

    private void Start()
    {
        tempDamage = damage;
        tempKnockBack = knockBack;
        damage = .5f;
        knockBack = 0;
        Vector2 startLoc;
        col = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        startLoc = player.transform.position;
        rotate(startLoc, transform.position);
        if(transform.position.x < player.transform.position.x)
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
    }

    private void Update()
    {
        if (active)
        {
            for (int i = 0; i < enemies.Count; i++) //TODO add catch for a null exception when the enemy dies during the animation
            {
                retry:
                try
                {
                    enemies[i].Translate(transform.position * Time.deltaTime * speed / (enemies[i].position - transform.position).magnitude);
                }
                catch (NullReferenceException) { 
                    enemies.RemoveAt(i);
                    if (enemies.Count != i) //this checks if there is an element after the dead enemy, if so, it jumps back to 'retry;
                        goto retry;         //in order to move the next affected enemy
                }  
            }
        }   
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
    public void enableCollider()
    {
        col.enabled = true;
    }
    public void startDamage() { damage = tempDamage; knockBack = tempKnockBack; }
    public void stop() { active = false; }
    public void disableCollider() { col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }
}
