using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class state //This is a base class for the finite State machine used for the skeleton AI 
{
    public enum STATE //This enum will hold the each state | currently a WIP
    {
        IDLE, WANDER, PURSUE, ATTACK, DEATH
    };

    public enum EVENT //This just holds 'stage' of each state, i.e. whether a state has just entered, is updating, or is exiting
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator animator; 
    protected Transform player; 
    protected state nextState; //this will hold the next stage the state machine will move to
    protected Seeker seeker; //This is a seeker script from the Astart Pathfinding project, it simplfies the pathfinding process

    float visDist = 10.0f; //Dist for the AI to activate
    float visAngle = 30.0f; //Half the angle for the ai to be able to view
    float attackDist = 2.75f; //The dist from which the ai can begin attacking

    public state(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player) 
    {
        npc = _npc;
        seeker = _seeker;
        animator = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; } //These methods get overriden by the substate classes, and when called through 'base.' move the state machine to the next stage
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public state Process() //This method just checks which stage the state is in and then calls that method
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool canSeePlayer() //This method checks if the Player is visible to the AI and returns true if so
    {
        float angle;
        Vector2 direction = player.position - npc.transform.position; //Gets the vector from the player to the AI
        if(npc.transform.localScale.x > 0)
            angle = Vector2.Angle(direction, npc.transform.right); //Takes the Angle between the AI's and the direction to the 
        else
            angle = Vector2.Angle(direction, -npc.transform.right);
        if (direction.magnitude < visDist && angle < visAngle) {//Checks if the player is within the AI's angle and range
            return true; } //Checks if the player is within the visible range and within the range of the player, Visangle is half of the range of vision
        return false;
    }
    public bool canAttack()//This method just checks if the player is within striking distance of the AI
    {
        Vector2 direction = player.position - npc.transform.position;
        if (direction.magnitude < attackDist)
            return true;
        return false;
    }
}

