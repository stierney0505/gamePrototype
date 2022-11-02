using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class idleState : state //Simple idle state, is the starting state of the machine
{
    float time = 0;
    public idleState(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player)
        : base(_npc, _seeker, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        animator.SetTrigger("idle");
        base.Enter();
    }

    public override void Update() //Either if the player is already in view switches to pursueState if not, waits 5 seconds and goes to wanderState
    {   
        if (canSeePlayer())
        {
            nextState = new pursueState(npc, seeker, animator, player);
            stage = EVENT.EXIT;
        }

        time += Time.deltaTime;
        if (time >= 5f)
        {
            nextState = new wanderState(npc, seeker, animator, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("idle");
        base.Exit();
    }

}