using Pathfinding;
using UnityEngine;

public class attackState : state
{
    public attackState(GameObject _npc, Seeker _seeker, Animator _anim, Transform _player)
        : base(_npc, _seeker, _anim, _player)
    {
        name = STATE.ATTACK;
    }

    public override void Enter()
    {
        animator.SetTrigger("attack");
        base.Enter();
    }

    public override void Update() //TODO make the AI remain within a certain range and go back to pursue if they are further than it
    {
        if (npc.GetComponent<SkeletonAI>().attackEnd) //This just simply checks if the attack is over before switching to the pursueState
        {
            nextState = new pursueState(npc, seeker, animator, player);
            npc.GetComponent<SkeletonAI>().attackEnd = false; //resets the bool checking if the attack ended
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("attack");
        base.Exit();
    }
}
