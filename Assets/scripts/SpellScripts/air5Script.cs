using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air5Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack, speed, loopLimit; 
    [SerializeField] char type; 
    internal CapsuleCollider2D capCol;
    internal PolygonCollider2D polyCol;
    float tempDamage, tempKnockBack; //these hold the damage values of damage and knockBack so that they can be activated after the movement portion of the spell
    List<Transform> enemies = new List<Transform>();
    bool active = true;
    int loops = 0;
    Animator animator;

    private void Start()
    {
        tempDamage = damage;
        tempKnockBack = knockBack;
        damage = .2f;
        knockBack = 0;
        animator = GetComponent<Animator>();
        polyCol = GetComponent<PolygonCollider2D>();
        capCol = GetComponent<CapsuleCollider2D>();
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
                    Vector2 direction = transform.position - enemies[i].position;
                    enemies[i].Translate(direction.normalized * Time.deltaTime * ((direction).magnitude) * 1.60f);
                }
                catch (NullReferenceException)
                {
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
    public void end() { }
    public float getDamage() { return damage; }
    public char getType() { return type; }

    public void enableCollider()
    {
        if(active)
            capCol.enabled = true; 
        else
            polyCol.enabled = true;
    }
    public void startDamage() { damage = tempDamage; knockBack = tempKnockBack; }
    public void stop() { active = false; }
    public void disableCollider() {
        if (active)
            capCol.enabled = true;
        else
            polyCol.enabled = true;
    }
    public float getKnockBack() { return knockBack; }

    public void loop()
    {
        if (loops < loopLimit)
            loops++;
        else {
            capCol.enabled = false;
            animator.SetTrigger("fade");
            active = false;
            startDamage();
            
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }
}
