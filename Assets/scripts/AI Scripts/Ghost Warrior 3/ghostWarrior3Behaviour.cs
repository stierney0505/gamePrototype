using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class ghostWarrior3Behaviour : MonoBehaviour, Unit //This ai uses a behaviour tree to decide what behaviour it will do, if it is not activated it will wander and if it is activated it never wander and try to attack ranged first 
{                                                         //then melee, if its out of range the behaviour tree will do nothing and the update method will have it approach the player until it can either attack or the player is dead
    Animator animator;
    Transform player;
    Seeker seeker;
    Rigidbody2D body;
    Collider2D melee1, melee2;
    char type = 'D';
    public float damage = 10.0f, health = 100.0f, attackCD = 3f, knockBack = 15f, speed = 4;
    bool meleePart2 = false, aiActivated = false, struck = false, endPath = false, attacking = false, playerDead = true;
    behaviourTree tree; //The above bools track if the melee attack will have two parts, if the ai is activated, if the ai is struck by the player and can't
    //move, if the ai is at the end of its path, if the ai is currently attacking, and if the player is Dead

    public enum ActionState { IDLE, WORKING };
    ActionState state = ActionState.IDLE;
    Node.Status treeStatus = Node.Status.RUNNING;

    float createPathTimer = .5f, pathTime = 0, rangedCD = 5; //pathTime is to keep track of how quickly the new path to the player is calculated, the pathTimer is .5 so the paths should only be calculated every .5 seconds, rather than every frame
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
        selectorNode action = new selectorNode("parent"); //Creation of the leaf and selector nodes for each action the ai will do
        Leaf wanderAction = new Leaf("wander", wander); //the wander action node
        selectorNode attackAction = new selectorNode("attack"); //the attack selectornode, it will choose whatever attack it can do or return failure
        Leaf rangedAtk = new Leaf("ranged1", ranged1); //rangedatk node
        Leaf meleeAtk = new Leaf("melee", meleeAttack1); //meleeAtk node

        attackAction.addChild(rangedAtk); //Setting up the behaviour tree, the behaviour tree should first try to wander if it can and if not
        attackAction.addChild(meleeAtk); //It tries to do an attack if it can and if not the ai 
        action.addChild(wanderAction); 
        action.addChild(attackAction);
        tree.addChild(action);

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {  //If there is no player, then the ai should do nothing and it shouldn't be in the movement animation   
            animator.SetBool("move", false);
            return;
        }

            if (body.velocity.sqrMagnitude <= 2) //First checks if the ai is experiencing knockback
        {
            animator.speed = 1;
            struck = false;
        }
        pathTime += Time.deltaTime; //Increments the timers for ranged attack cooldown and the path timer
        rangedCD += Time.deltaTime;

        if (path != null && !struck && !attacking) //This checks if there is a path, and the ai isn't struck and if its not attacking, and if so it calls the travel method
        {
            travel();
            state = ActionState.WORKING;
        }

        if (!struck) //if the ai is not struck then it should process through the behaviour tree
                treeStatus = tree.Process();

        if (aiActivated && pathTime >= createPathTimer) { pathTime = pathTime - createPathTimer; createPath(); } //These lines just check if the time between calculations is .5 seconds or more and then calculates a new path to the player

        if (!aiActivated) //this just checks if the ai can see the player while inactive and if it can then it activates
            aiActivated = canSeePlayer();
    }


    public char getType() { return type; }
    public float getDamage() { return damage; }
    public float getKnockBack() { return knockBack; }

    public void takeDamage(spell spell) //This method does the damage calculation for when a spell strikes the AI
    {
        deactivateMelee1Hitbox(); //disables hitboxes while struck
        deactivateMelee2Hitbox();

        float damage = spell.getDamage(); //gets the type and damage from the spell that is damaging it
        char atkType = spell.getType();

        GameObject hitEffect = Instantiate(Resources.Load(spellTypeHelper.getOnHitEffect(spell.getType()))) as GameObject; //creates a hiteffect
        hitEffect.transform.position = transform.position; //set hit effect position to itself
        health -= spellTypeHelper.damageModifier(atkType, type, damage); //Subtracts the health from the enemy based after modifying it based on its types
        aiActivated = true; //if the ai gets struck it activates it stops if from attacking
        attacking = false; 
        animator.SetTrigger("hit"); 
        animator.SetBool("move", false);
        struck = true;
        if (health <= 0) //Checks if the health is beneath 0 and if so trigger the death method
            die();
    }

    public void takeDamage(float damage) //This method does the damage calculation when the AI collides with a structure, or other non spell damage
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
        body.AddForce(dir * knockBack, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col) //This decrements the HP of the ai, and generates a hit effect | TODO put this into a helper method and add damage calculation based upon damage type
    {
        if (col.tag == "Spell" && col.gameObject.TryGetComponent<spell>(out spell spellComponent))
        {
            spellComponent.end(false); //Triggers the end method to end the start the ending animation of the attacking spells, also with a false environment flag to let it know not to remove it immediately
            spellComponent.addEnemy(transform);
            if (spellComponent.getDamage() > 0)
            {
                takeDamage(spellComponent); //calls takeDamage method to apply damage
                float knockBack = spellComponent.getKnockBack();
                if (knockBack > 0)
                {
                    Vector2 forceDirection = (transform.position - col.transform.position).normalized; //Computes the direction vector between the spell and the ai 
                    takeKnockBack(forceDirection, knockBack);//Applies the knockback through the knockback method
                }
            }
        }
    }

    public Node.Status meleeAttack1() //This melee attack has a starting swing and then a 2 in 7 chance to hit right aftwards
    {
        Vector2 dist = player.transform.position - transform.position;
         
  
        if (state == ActionState.IDLE) //if the action state is idle the melee attack has completed so it returns success for the status node
        {
            return Node.Status.SUCCESS;
        }
        else if (attacking)
        {
            return Node.Status.RUNNING;//if the attacking bool is true or the melee part 2 is true it returns running for the status node
        }
        else if(meleePart2) 
            return Node.Status.RUNNING;
        if (dist.sqrMagnitude <= 12 && meleePart2 == false) //This checks if the dist is about 3.46 away and its not doing a melee part two, if it hasn't returned yet it starts the melee attack melee
        {
            animator.SetTrigger("melee1");
            attacking = true;
            state = ActionState.WORKING; //sets the state to working so that it knows it currently is performing an action
            int rand = Random.Range(0, 7); //Random number to later calculate if the ai will do another attack
            if (rand <= 1)
            {
                meleePart2 = true;
                return Node.Status.RUNNING;
            }
        }
        
         return Node.Status.FAILURE; //If it hasn't returned by now it is not close enough and thus it cannot melee attack

    }
    public void activateMelee1Hitbox() { melee1.enabled = true;  attacking = true; } //Enables hitbox and makes the attacking bool true because its attacking
    public void activateMelee2Hitbox() { melee2.enabled = true; } //Because melee2 only happens after melee1 attacking bool doesn't need to be turnt to true
    public void checkMelee2() //This helper method just trigger the ai to start melee2 attack if meleePart2 bool is true
    {
        if (meleePart2)
            animator.SetBool("melee2", true);
    }
    public void deactivateMelee1Hitbox() { melee1.enabled = false; } 
    public void deactivateMelee2Hitbox() { melee2.enabled = false; animator.SetBool("melee2", false); meleePart2 = false; } //Sets meleepart2 to false so that endAttack() will disable the hitbox when called through the animator
    public Node.Status ranged1() //The ranged attack method, checks if the ranged CD is up and then has a 3/5 chance to do ranged 1 and a 2/5 chance to return ranged()
    {
        if (state == ActionState.IDLE ) //if the state is idle the ranged attack has finished and returns success
        {
            return Node.Status.SUCCESS;
        }
        else if (attacking) //if the ai is attacking then returns running
            return Node.Status.RUNNING;
        if (rangedCD >= 5) //Checks for the CD in order to do a ranged attack
        {
            state = ActionState.WORKING; //switches the state to working
            rangedCD = 0; //resets rangedCD
            float dist = (player.transform.position - transform.position).sqrMagnitude; //Calculates the squared magnitude of the distance between the player and the ai

            if (dist >= 4.1 && dist < 197) //If the ai is further than 4.1 (meleeRange) and closer than ~14 than it triggers a ranged attack
            {
                int rand = Random.Range(0, 5); //This random is to check if the ranged attack will be ranged1 or ranged2 
                if (rand <= 1) { return ranged2(); } //2/5 chance for ranged 2
                animator.SetTrigger("ranged1"); 
                attacking = true; //These three just start ranged1 animation and return running
                return Node.Status.RUNNING;
            }
        }
        return Node.Status.FAILURE;
    }

    public void createRanged1() //Animator method to create the ranged projectile
    {
        Vector2 rangedStart = transform.GetChild(2).position;
        GameObject rangedAttack = Instantiate(Resources.Load("Enemies/EnemyProjectiles/GhostWarrior3Projectile1") as GameObject);
        rangedAttack.transform.position = rangedStart;

    }
    public Node.Status ranged2() //Ranged 2 attack, just starts ranged2 animation and returns running
    {
        animator.SetTrigger("ranged2");
        attacking = true;
        return Node.Status.RUNNING;
    }
    public void createRanged2() //Animator method to create the ranged2 projectiles
    {
        Vector2 rangedStart = transform.GetChild(3).position;
        GameObject rangedAttack = Instantiate(Resources.Load("Enemies/EnemyProjectiles/GhostWarrior3Projectile2") as GameObject);
        rangedAttack.transform.position = rangedStart;
    }

    public void die() //Kills the ai, deactivates its hitboxes, starts the death animation, and sets speed to 0 to stop traveling
    {
        Collider2D hitbox = GetComponent<Collider2D>();
        deactivateMelee2Hitbox();
        deactivateMelee1Hitbox();
        hitbox.enabled = false;
        speed = 0;
        animator.SetTrigger("death");
    }
    public void remove() { Destroy(gameObject); } //Animator method to destroy the ai game object

    public void setAnimSpeedZero() { animator.speed = 0; }

    void OnPathComplete(Pathfinding.Path p) //This method runs everything a path is completed, and it checks if there are no errors in the pathfinding
    {
        if (!p.error)
        { path = p; currentWayPoint = 0; }
    }

    void travel() //travel script, it basically checks if the waypoints are over or if the player is too close and then moves the ai towards the player
    {
        if (speed == 0) //if speed is 0 it shouldn't be traveling
            return;
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

    public Node.Status wander() //Wander method, 1 in three chance to wander every 5 seconds
    {   //TODO maybe make the AI wander after the player dies????
        if (aiActivated) //If the ai is activated it should return failure because it won't wander, it will be chasing/attacking the player
            return Node.Status.FAILURE;
        else if (attacking) //If the ai is attacking the tree is in the process of attacking so it returns running
            return Node.Status.RUNNING;
        
        if (pathTime >= 5 && endPath == true) //This checks if the ai should find a new path, pathTime is a timer that checks every 5 seconds to find a new wander path
        {
            int rand = Random.Range(0, 3); //creates a random number to see if the ai will wander, 1/3 chance to wander
            if (rand == 1) { endPath = false; seeker.StartPath(transform.position, getWanderTarget(), OnPathComplete); } //Sets endpath =false because the ai will be moving soon, and calls the getWanderTarget() method
            state = ActionState.WORKING;
            pathTime = 0f;
        }
        else if (endPath == true) //If the path is at the end the state should be idle because the ai is not currently doing anything
            state = ActionState.IDLE;

        return Node.Status.RUNNING; //if it hasn't returned yet then it should return running
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

    void endAttack() { //This method is called after attacks to change attacking to false, it is needed because melee2 only sometimes happens 
        if (!meleePart2) //after melee1, so a seperate method is needed to turn attacking to false when melee2 won't be starting or when melee2 is finished
        {
            attacking = false;
            state = ActionState.IDLE;
        }
    }
}
