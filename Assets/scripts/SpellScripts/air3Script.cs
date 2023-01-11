using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class air3Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack, speed; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    internal CircleCollider2D cirCol;
    internal CapsuleCollider2D capCol;
    List<Transform> enemies = new List<Transform>();
    float angle = 60, tempDamge, tempKnockBack; //Temp damage and knockback are to hold the values of damage and knockback during the wind up
    bool active = true; //This bool is to track when the spell is 'active' i.e. when it should be moving enemies

    private void Start()
    {   
       tempDamge = damage; tempKnockBack = knockBack;
       damage = .5f; knockBack = 0;
       cirCol = GetComponent<CircleCollider2D>();
       capCol = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (active)
        {
            for (int i = 0; i < enemies.Count; i++) //TODO add catch for a null exception when the enemy dies during the animation
            {
                Vector3 direction = transform.position - enemies[i].position;
                direction = Quaternion.Euler(0, 0, angle) * direction; //The third number defines how quickly the enemy moves into the center
                float appliedSpeed = speed * Time.deltaTime;

            retry:
                try
                {
                    enemies[i].transform.Translate(-1 * (Vector2)direction.normalized * appliedSpeed, Space.World);
                }
                catch (NullReferenceException)
                {
                    enemies.RemoveAt(i);
                    if (enemies.Count != i) //this checks if there is an element after the dead enemy, if so, it jumps back to 'retry;
                        goto retry;         //in order to move the next affected enemy
                }
            }
        }
        angle -= 2;
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
    public void enableCollider()
    {
        cirCol.enabled = false;
        damage = tempDamge;
        knockBack = tempKnockBack;
        capCol.enabled = true;
    }
    public void disableCollider() { capCol.enabled = false; }
    
    public float getKnockBack() { return knockBack; }

    public void stop() { active = false; }

    public void addEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }
}
