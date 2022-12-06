using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class air6Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack, speed, loopLimit;
    [SerializeField] char type;
    internal Collider2D col;
    Vector2 startLoc, dir;
    float time = 0, x, y; //these hold the damage values of damage and knockBack so that they can be activated after the movement portion of the spell
    List<Transform> enemies = new List<Transform>();
    int loops = 0;
    Animator animator;

    private void Start()
    {
        startLoc = transform.position;
        knockBack = 0;
        knockBack = 0;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        x = 3 * Mathf.Cos(time);
        y = 3 * Mathf.Sin(2 * time) / 2;
        dir = new Vector2(startLoc.x + x, startLoc.y + y) - (Vector2)transform.position;
        transform.Translate(dir * Time.deltaTime * speed);

            for (int i = 0; i < enemies.Count; i++) //TODO add catch for a null exception when the enemy dies during the animation
            {
            retry:
                try
                {
                    dir = transform.position - enemies[i].position;
                    enemies[i].Translate(dir.normalized * Time.deltaTime * .2f);
                }
                catch (NullReferenceException)
                {
                    enemies.RemoveAt(i);
                    if (enemies.Count != i) //this checks if there is an element after the dead enemy, if so, it jumps back to 'retry;
                        goto retry;         //in order to move the next affected enemy
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
        col.enabled = true;
    }
    public void disableCollider()
    {
        col.enabled = false;
    }

    public void loop()
    {
        if (loops < loopLimit)
            loops++;
        else
        {
            animator.SetTrigger("fade");
        }
    }
    public float getKnockBack() { return knockBack; }
    public void addEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }
}
