using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class deathState : state //very simple state that is used to stop the ai from doing anything but the death animation
{
    public deathState(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player)
        : base(_npc, _seeker, _anim, _player)
    {
        name = STATE.DEATH;
    }

    public override void Enter()
    {
        animator.SetTrigger("death");
        base.Enter();
    }

    public override void Update()
    {
        

    }

    public override void Exit()
    {
        
    }
}
