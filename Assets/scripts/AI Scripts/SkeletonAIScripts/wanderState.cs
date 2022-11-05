using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class wanderState : state //This wander state has a 1 in 4 chance to travel to a random location every  3.5 seconds
{
    Path path;
    int currentWayPoint = 0;
    bool endPath = false;
    float wanderIdleTimer = 3.5f, time = 0f, speed = 2f; //The wanderIdle Timer is the time limit beyond wanders, the time float is to measure the time between wanders and speed is the wander speed
    
    
    public wanderState(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player)
        : base(_npc, _seeker, _anim, _player)
    {
        name = STATE.WANDER;
    }

    public override void Enter()//Imediately upon entering the ai will wander
    {
        animator.SetBool("move", true);
        base.Enter();
        seeker.StartPath(npc.transform.position, getWanderTarget(), OnPathComplete);
    }

    public override void Update() //each update the AI checks if it can see the player, and if not either travels along the wander path or checks if it should get a new path
    {   
        if (canSeePlayer() || npc.GetComponent<SkeletonAI>().aiTriggerd)
        {
            nextState = new pursueState(npc, seeker, animator, player);
            stage = EVENT.EXIT;
            return;
        }
        if (!endPath) { //This checks if the ai is at the end of its path, if not it keeps traveling
            travel(); 
        }
        else //This increments the time float and if its above 3.5f there is a 1 in 4 chance to find a new path
        {
            time += Time.deltaTime;
            if (time >= wanderIdleTimer)
            {
                int rand = Random.Range(0, 4);
                if (rand == 0) { endPath = false; seeker.StartPath(npc.transform.position, getWanderTarget(), OnPathComplete); }
                time = 0f;
            }
        }


    }

    public override void Exit()
    {
        animator.SetBool("move", false);
        base.Exit();
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)//checks if a path is found
        { path = p; currentWayPoint = 0; }
        else //If the path generated cannot be traveled this method will call until a path is found
            seeker.StartPath(npc.transform.position, getWanderTarget(), OnPathComplete);
    }

    void travel() //Shamelessly copy pasted from pursueState,
    {
        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        { endPath = true; animator.SetBool("move", false); animator.SetTrigger("idle"); return; }
        else { endPath = false; animator.SetBool("move", true); }

        Vector2 direction = path.vectorPath[currentWayPoint] - npc.transform.position;
        if (path.vectorPath[path.vectorPath.Count - 1].x > npc.transform.position.x && npc.transform.localScale.x < 0) { npc.transform.localScale = new Vector2(-npc.transform.localScale.x, npc.transform.localScale.y); }
        else if (path.vectorPath[path.vectorPath.Count - 1].x < npc.transform.position.x && npc.transform.localScale.x > 0) { npc.transform.localScale = new Vector2(-npc.transform.localScale.x, npc.transform.localScale.y); }

        npc.transform.Translate(direction.normalized * speed * Time.deltaTime);

        float wayPointDist = Vector2.Distance(npc.transform.position, path.vectorPath[currentWayPoint]);
        if (wayPointDist < 1f) //Currently hard coded the minimun distance to waypoints TODO find a proper distance for waypoints
        {
            currentWayPoint++;
        }
    }

    public Vector3 getWanderTarget() //This method gets a vector to a random point around the ai and returns that vector
    {
        float wanderRadius = Random.Range(4f, 12f); //This is the radius of the circle for the AI to wander to
        Vector2  wanderTarget = new Vector2(Random.Range(-1.00f, 1.00f), Random.Range(-1.00f, 1.00f)); //This creates a vector with a random direction

        wanderTarget.Normalize(); //normalizes the vector so we can use it as a direction
        wanderTarget *= wanderRadius; //this multiples the direction vector by the wander radius as the destination
        return wanderTarget;
    }
}
