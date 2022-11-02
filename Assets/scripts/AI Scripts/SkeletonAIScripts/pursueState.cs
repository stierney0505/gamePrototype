using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class pursueState : state
{   
    private float time = 0f, speed = 4f, createPathTimer = .5f; //time is to keep track of how quickly the new path to the player is calculated, the pathTimer is .5 so the paths should only be calculated every .5 seconds, rather than every frame
    Path path; //the path the AI has
    int currentWayPoint = 0; //The current waypoint of the path they are on
    bool endPath = false; //To check if the AI is at the end of their path
    public pursueState(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player)
        : base(_npc, _seeker, _anim, _player)
    {
        name = STATE.PURSUE;
    }

    public override void Enter()
    {
        animator.SetBool("move", true); 
        animator.speed = 1.4f; //Speeds the animator up to be match the speed at which the AI moves at
        base.Enter();
        seeker.StartPath(npc.transform.position, player.position, OnPathComplete); //Calculates a path to the player
        
    }
    public override void Update()
    {   
        time += Time.deltaTime;
        if(time >= createPathTimer) { time = time - createPathTimer; createPath(); } //These lines just check if the time between calculations is .5 seconds or more and then calculates a new path to the player

        travel(); //Travel method for moving to waypoints

        if (canAttack() && npc.GetComponent<SkeletonAI>().canAttack)//Checks if the player is in range to attack and if the cooldown for the attack is ready, then switches to attack state
        {
            nextState = new attackState(npc, seeker, animator, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        animator.SetBool("move", false);
        animator.speed = 1f; //resets animator speed
        base.Exit();
        
    }

    void OnPathComplete(Path p) //This method runs everything a path is completed, and it checks if there are no errors in the pathfinding
    {
        if (!p.error) 
        { path = p; currentWayPoint = 0; }
    }

    void travel() //travel script, it basically checks if the waypoints are over or if the player is too close and then moves the ai towards the player
    {   
        if (path == null) //ensures there is a path to follow
            return;

        Vector2 playerDist = player.position - npc.transform.position; //to get the distance between the player and the AI

        if (currentWayPoint >= path.vectorPath.Count || playerDist.sqrMagnitude > 4 ) //This checks if the waypoint is not the last, and also if the AI is closer than 2 units away
        { endPath = true; return; }
        else { endPath = false; } //otherwise we arent at the end of our path

        Vector2 direction = path.vectorPath[currentWayPoint] - npc.transform.position; //This calculates the vector between the next waypoint and the AI


        if (path.vectorPath[path.vectorPath.Count - 1].x > npc.transform.position.x && npc.transform.localScale.x < 0) {npc.transform.localScale = new Vector2(-npc.transform.localScale.x, npc.transform.localScale.y);} //These two long statements just flip the sprite around depending if they are 
        else if(path.vectorPath[path.vectorPath.Count - 1].x < npc.transform.position.x && npc.transform.localScale.x > 0) { npc.transform.localScale = new Vector2(-npc.transform.localScale.x, npc.transform.localScale.y); } //moving left or right
        
        npc.transform.Translate(direction.normalized * speed * Time.deltaTime); //Actually moves the npc

        float wayPointDist = Vector2.Distance(npc.transform.position, path.vectorPath[currentWayPoint]); //gets the dist between the waypoint and the AI
        if(wayPointDist < 2f) //Currently hard coded the minimun distance to waypoints TODO find a proper distance for waypoints
        {
            currentWayPoint++;
        }
    }

    void createPath() //Probably unneccessary but this method checks if the seeker is done calculating a path then if so calculates a new one.
    {
        if (seeker.IsDone()) 
            seeker.StartPath(npc.transform.position, player.position, OnPathComplete);
    }
}
