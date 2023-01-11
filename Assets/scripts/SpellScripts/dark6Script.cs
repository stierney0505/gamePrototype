using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dark6Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack, speed; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    internal Collider2D col;
    List<Transform> enemies = new List<Transform>();
    bool active = true; //This bool is to track when the spell is 'active' i.e. when it should be moving enemies


    private void Start()
    {
        int rand = UnityEngine.Random.Range(-60, 61);
        transform.Rotate(0, 0, rand);
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (active) {
            for (int i = 0; i < enemies.Count; i++)
            {

            retry:
                try
                {
                    Vector3 direction = transform.position - enemies[i].position;
                    direction = Quaternion.Euler(0, 0, 80) * direction; //The third number defines how quickly the enemy moves into the center
                    float appliedSpeed = speed * Time.deltaTime;
                    enemies[i].transform.Translate((Vector2)direction.normalized * appliedSpeed, Space.World);
                }
                catch (MissingReferenceException)
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
     public dLRSNode.types getType() { return type; }
    public void enableCollider()
    {
        col.enabled = true;
    }
    public void disableCollider() { col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void increaseDmg() { damage *= 3; knockBack = 200; active = false; }

    public void addEnemy(Transform enemyTransform)
    {
        enemies.Add(enemyTransform);
    }
}
