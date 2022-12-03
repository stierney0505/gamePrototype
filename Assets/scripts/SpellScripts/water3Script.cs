using Newtonsoft.Json.Bson;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class water3Script : MonoBehaviour, spell
{

    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] char type; //to decide the spell type
    public float speed;
    Seeker seeker;
    GameObject targetEnemy;
    Path path;
    Animator animator;
    int currentWayPoint = 0, loops = 0;
    float time;
    bool noEnemies = false, detonated = false;
    

    private void Start()
    {     
        animator= GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        findEnemy();
        if(!noEnemies)
            seeker.StartPath(transform.position, targetEnemy.transform.position, OnPathComplete);
    }

    private void Update()
    {
        if (!noEnemies && !detonated)
        {
            if (targetEnemy == null || targetEnemy.Equals(null)) //Checks if the enemy still exists
            {
                findEnemy();
            }

            time += Time.deltaTime;
            if (time >= .3f) { time = 0; createPath(); }

            travel();
        }
        else //Every 1.25 seconds checks if there are enemies if the noEnemies bool is true (in case they spawned)
        {
            time += Time.deltaTime;
            if(time >= 1.25f)
            {
                findEnemy();
                time = 0;
            }
        }

        if(loops > 8) { animator.SetTrigger("fade"); detonated = true; }
    }

    private void OnPathComplete(Path p) //This method runs everything a path is completed, and it checks if there are no errors in the pathfinding
    {
        if (!p.error)
        { path = p; currentWayPoint = 0; }
    }

    private void travel()
    {
        if (path == null) //ensures there is a path to follow
            return;

        Vector2 direction = path.vectorPath[currentWayPoint] - this.transform.position;
        
        
        this.transform.Translate(direction.normalized * speed * Time.deltaTime);

        float wayPointDist = Vector2.Distance(this.transform.position, path.vectorPath[currentWayPoint]); //gets the dist between the waypoint and the AI
        if (wayPointDist < 1f)
        {
            currentWayPoint++;
        }
    }

    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void remove() { Destroy(gameObject); }
    public void end() { disableCollider(); animator.SetTrigger("fade"); detonated = true; }
    public float getDamage() { return damage; }
    public char getType() { return type; }

    public void findEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
            targetEnemy = enemies[0];
        else { targetEnemy= null; noEnemies = true;  return; }

        Vector2 distVector;
        for (int i = 1; i < enemies.Length; i++)
        {
            Vector2 testVector = transform.position - enemies[i].transform.position;
            distVector = transform.position - targetEnemy.transform.position;
            if (distVector.sqrMagnitude < testVector.sqrMagnitude) { 
                targetEnemy = enemies[i]; 
                if(targetEnemy.transform.position.x < transform.position.x)
                    transform.eulerAngles = new Vector2(0, 180);
            }
        }

    }

    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
        if(detonated)
            damage = 100;
    }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    private void createPath() //Probably unneccessary but this method checks if the seeker is done calculating a path then if so calculates a new one.
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, targetEnemy.transform.position, OnPathComplete);
    }

    public void loop() { loops++; }
    public void setDetonated() { detonated = true; }
}
