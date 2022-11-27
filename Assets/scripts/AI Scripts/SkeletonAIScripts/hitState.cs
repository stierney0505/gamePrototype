using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitState : state
{
    Rigidbody2D rigidbody;
    public hitState(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player, PlayerScript _playerScr)
        : base(_npc, _seeker, _anim, _player, _playerScr)
    {
        name = STATE.HIT;
    }

    public override void Enter()
    {
        rigidbody = npc.GetComponent<Rigidbody2D>();    
        animator.SetTrigger("damage");
        animator.SetBool("moving", false);
        base.Enter();
        nextState = new pursueState(npc, seeker, animator, player, playerScr);
    }

    public override void Update() //Either if the player is already in view switches to pursueState if not, waits 5 seconds and goes to wanderState
    {   
        if(rigidbody.velocity.sqrMagnitude <= 1)
            animator.speed = 1;
        
        
    }

    public override void Exit()
    {
        
        animator.ResetTrigger("damage");
        base.Exit();
    }


}
