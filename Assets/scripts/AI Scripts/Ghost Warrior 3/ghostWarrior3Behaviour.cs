using Newtonsoft.Json.Bson;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ghostWarrior3Behaviour : MonoBehaviour, Unit
{
    Animator animator;
    Transform player;
    Seeker seeker;
    Rigidbody2D body;
    Collider2D melee1, melee2;
    char type = 'D';
    public float damage = 10.0f, health = 100.0f, attackCD = 3f, knockBack = 15f, speed = 4;
    bool meleePart2 = false, aiActivated = false, struck = false, endPath = false, attacking = false, playerDead = true;
    behaviourTree tree;

    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;
    Node.Status treeStatus = Node.Status.RUNNING;

    float createPathTimer = .5f, time = 0, rangedCD = 5; //time is to keep track of how quickly the new path to the player is calculated, the pathTimer is .5 so the paths should only be calculated every .5 seconds, rather than every frame
    Pathfinding.Path path; //the path the AI has
    int currentWayPoint = 0; //The current waypoint of the path they are on

    // Start is called before the first frame update
    void Start()
    {
        melee1 = transform.GetChild(0).GetComponentInChildren<Collider2D>();
        melee2 = transform.GetChild(1).GetComponentInChildren<Collider2D>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        if (players.Length != 0) //If there are no players for whatever reason the AI will just not create a state machine and stand there
        {
            player = players[0].transform;
            playerDead = false;
        }

        tree = new behaviourTree();
        selectorNode action = new selectorNode("parent");
        Leaf wanderAction = new Leaf("wander", wander);
        selectorNode attackAction = new selectorNode("attack");
        Leaf rangedAtk = new Leaf("ranged1", ranged1);
        Leaf meleeAtk = new Leaf("melee", meleeAttack1);

        attackAction.addChild(rangedAtk);
        attackAction.addChild(meleeAtk);
        action.addChild(wanderAction); 
        action.addChild(attackAction);
        tree.addChild(action);

    }

    // Update is called once per frame
    void Update()
    {
        if (body.velocity.sqrMagnitude <= 2)
        {
            animator.speed = 1;
            struck = false;
        }
        time += Time.deltaTime;
        rangedCD += Time.deltaTime;

        if (path != null && !struck && !attacking)
        {
            travel();
            state = ActionState.WORKING;
        }


        if (player == null)
            playerDead = true;

        if(!playerDead){ 
            if (!struck)
                treeStatus = tree.Process();

            if (aiActivated && time >= createPathTimer) { time = time - createPathTimer; createPath(); } //These lines just check if the time between calculations is .5 seconds or more and then calculates a new path to the player

        }
    }

    private void FixedUpdate()
    {
        if (!aiActivated)
            aiActivated = canSeePlayer();
    }

    public char getType() { return type; }
    public float getDamage() { return damage; }
    public float getKnockBack() { return knockBack; }

    public void takeDamage(spell spell)
    {
        deactivateMelee1Hitbox();
        deactivateMelee2Hitbox();

        float damage = spell.getDamage();
        char type = spell.getType();

        GameObject hitEffect = Instantiate(Resources.Load(spellTypeHelper.getOnHitEffect(spell.getType()))) as GameObject;
        hitEffect.transform.position = transform.position;
        health -= damage;
        aiActivated = true;
        attacking = false;
        animator.SetTrigger("hit");
        animator.SetBool("move", false);
        struck = true;
        if (health <= 0)
            die();
    }

    public void takeDamage(float damage)
    {
        deactivateMelee1Hitbox();
        deactivateMelee2Hitbox();

        health -= damage;
        aiActivated = true;
        attacking = false;
        animator.SetTrigger("hit");
        animator.SetBool("move", false);
        struck = true;
        if (health <= 0)
            die();

    }

    public void takeKnockBack(Vector2 dir, float knockBack)
    {
        body.AddForce(dir.normalized * knockBack, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col) //This decrements the HP of the ai, and generates a hit effect | TODO put this into a helper method and add damage calculation based upon damage type
    {
        if (col.tag == "Spell" && col.gameObject.TryGetComponent<spell>(out spell spellComponent))
        {
            spellComponent.end(false);
            spellComponent.addEnemy(transform);
            if (spellComponent.getDamage() > 0)
            {
                takeDamage(spellComponent);
                float knockBack = spellComponent.getKnockBack();
                if (knockBack > 0)
                {
                    Vector2 forceDirection = transform.position - col.transform.position;
                    takeKnockBack(forceDirection, knockBack);
                }
            }
        }
    }

    public Node.Status meleeAttack1()
    {
        Vector2 dist = player.transform.position - transform.position;
         
  
        if (state == ActionState.IDLE)
        {
            return Node.Status.SUCCESS;
        }
        else if (attacking)
        {
            return Node.Status.RUNNING;
        }
        else if(meleePart2)
            return Node.Status.RUNNING;
        if (dist.sqrMagnitude <= 12 && meleePart2 == false)
        {
            animator.SetTrigger("melee1");
            attacking = true;
            state = ActionState.WORKING;
            int rand = Random.Range(0, 7);
            if (rand <= 1)
            {
                meleePart2 = true;
                return Node.Status.RUNNING;
            }
        }
        
         return Node.Status.FAILURE;

    }
    public void activateMelee1Hitbox() { melee1.enabled = true;  attacking = true; }
    public void activateMelee2Hitbox() { melee2.enabled = true; }
    public void checkMelee2()
    {
        
        if (meleePart2)
            animator.SetBool("melee2", true);
    }
    public void deactivateMelee1Hitbox() { melee1.enabled = false; }
    public void deactivateMelee2Hitbox() { melee2.enabled = false; animator.SetBool("melee2", false); meleePart2 = false; }
    public Node.Status ranged1()
    {
        if (state == ActionState.IDLE )
        {
            return Node.Status.SUCCESS;
        }
        else if (attacking)
            return Node.Status.RUNNING;
        if (rangedCD >= 5)
        {
            state = ActionState.WORKING;
            rangedCD = 0;
            float dist = (player.transform.position - transform.position).sqrMagnitude;

            if (dist >= 4.1 && dist < 197)
            {
                int rand = Random.Range(0, 5);
                if (rand <= 1) { return ranged2(); }
                animator.SetTrigger("ranged1");
                attacking = true;
                return Node.Status.RUNNING;
            }
        }
        return Node.Status.FAILURE;
    }

    public void createRanged1()
    {
        Vector2 rangedStart = transform.GetChild(2).position;
        GameObject rangedAttack = Instantiate(Resources.Load("Enemies/EnemyProjectiles/GhostWarrior3Projectile1") as GameObject);
        rangedAttack.transform.position = rangedStart;

    }
    public Node.Status ranged2()
    {
        animator.SetTrigger("ranged2");
        attacking = true;
        return Node.Status.RUNNING;
    }
    public void createRanged2()
    {
        Vector2 rangedStart = transform.GetChild(3).position;
        GameObject rangedAttack = Instantiate(Resources.Load("Enemies/EnemyProjectiles/GhostWarrior3Projectile2") as GameObject);
        rangedAttack.transform.position = rangedStart;
    }

    public void die()
    {
        Collider2D hitbox = GetComponent<Collider2D>();
        deactivateMelee2Hitbox();
        deactivateMelee1Hitbox();
        hitbox.enabled = false;
        speed = 0;
        animator.SetTrigger("death");
    }
    public void remove() { Destroy(gameObject); }

    public void setAnimSpeedZero() { animator.speed = 0; }

    void OnPathComplete(Pathfinding.Path p) //This method runs everything a path is completed, and it checks if there are no errors in the pathfinding
    {
        if (!p.error)
        { path = p; currentWayPoint = 0; }
    }

    void travel() //travel script, it basically checks if the waypoints are over or if the player is too close and then moves the ai towards the player
    {

        if (path == null) //ensures there is a path to follow
            return;

        Vector2 playerDist = player.position - transform.position; //to get the distance between the player and the AI

        if (currentWayPoint >= path.vectorPath.Count || playerDist.sqrMagnitude < 4) //This checks if the waypoint is not the last, and also if the AI is closer than 2 units away
        { endPath = true; return; }
        else { endPath = false; } //otherwise we arent at the end of our path

        Vector2 direction = path.vectorPath[currentWayPoint] - transform.position; //This calculates the vector between the next waypoint and the AI


        if (path.vectorPath[path.vectorPath.Count - 1].x > transform.position.x && transform.localScale.x < 0) { transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); } //These two long statements just flip the sprite around depending if they are 
        else if (path.vectorPath[path.vectorPath.Count - 1].x < transform.position.x && transform.localScale.x > 0) { transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); } //moving left or right

        transform.Translate(direction.normalized * speed * Time.deltaTime); //Actually moves the npc
        animator.speed = 1;
        animator.SetBool("move", true);

        float wayPointDist = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]); //gets the dist between the waypoint and the AI
        if (wayPointDist < 2f) //Currently hard coded the minimun distance to waypoints TODO find a proper distance for waypoints
        {
            currentWayPoint++;
        }
    }

    void createPath() //Probably unneccessary but this method checks if the seeker is done calculating a path then if so calculates a new one.
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, player.position, OnPathComplete);
    }

    public Node.Status wander()
    {
        if (attacking)
            return Node.Status.RUNNING;
        else if (aiActivated)
            return Node.Status.FAILURE;
        
        if (time >= 5)
        {
            int rand = Random.Range(0, 4);
            if (rand == 0) { endPath = false; seeker.StartPath(transform.position, getWanderTarget(), OnPathComplete); }
            state = ActionState.WORKING;
            time = 0f;
        }
        else if (endPath == true)
            state = ActionState.IDLE;

        return Node.Status.RUNNING;
    }

    public Vector3 getWanderTarget() //This method gets a vector to a random point around the ai and returns that vector
    {
        float wanderRadius = Random.Range(4f, 12f); //This is the radius of the circle for the AI to wander to
        Vector2 wanderTarget = new Vector2(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f)); //This creates a vector with a random direction

        wanderTarget.Normalize(); //normalizes the vector so we can use it as a direction
        wanderTarget *= wanderRadius; //this multiples the direction vector by the wander radius as the destination
        return wanderTarget;
    }

    public bool canSeePlayer() //This method checks if the Player is visible to the AI and returns true if so
    {
        float angle;
        Vector2 direction = player.position - transform.position; //Gets the vector from the player to the AI
        if (transform.localScale.x > 0)
            angle = Vector2.Angle(direction, transform.right); //Takes the Angle between the AI's and the direction to the 
        else
            angle = Vector2.Angle(direction, -transform.right);
        if (direction.magnitude < 10 && angle < 30) //Hard coded angle and visual distance
        {//Checks if the player is within the AI's angle and range
            return true;
        } //Checks if the player is within the visible range and within the range of the player, Visangle is half of the range of vision
        return false;
    }

    void endAttack() {
        if (!meleePart2)
        {
            attacking = false;
            state = ActionState.IDLE;
        }
    }
}
