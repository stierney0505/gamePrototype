using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air4Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack, speed; //to decide the damage number
    [SerializeField] char type; //to decide the spell type
    [SerializeField] int loopLimit;
    internal Collider2D col;
    List<Transform> enemies = new List<Transform>();
    int loops = 0;
    bool active = true; //This bool is to track when the spell is 'active' i.e. when it should be moving enemies
    Animator animator;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (active)
        {
            for (int i = 0; i < enemies.Count; i++) //TODO add catch for a null exception when the enemy dies during the animation
            {
                Vector3 direction = transform.position - enemies[i].position;
                direction = Quaternion.Euler(0, 0, 91) * direction; //The third number defines how quickly the enemy moves into the center
                float appliedSpeed = speed * Time.deltaTime;

            retry:
                try
                {
                    enemies[i].transform.Translate((Vector2)direction.normalized * appliedSpeed, Space.World);
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
    public void end(bool environment)
    {
        if (environment)
            remove();
    }
    public float getDamage() { return damage; }
    public char getType() { return type; }
    public void enableCollider()
    {
        col.enabled = true;
    }
    public void disableCollider() { col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void stop() { active = false; }

    public void loop() {
        if (loops < loopLimit)
            loops++;
        else
            animator.SetTrigger("fade");
    }

    public void addEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }
}
