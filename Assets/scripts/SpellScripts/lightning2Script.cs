using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning2Script : spell
{
    private Animator animator;
    int slowOrFast = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public override Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public override void end()
    {   
        Destroy(gameObject);
    }

    public void setSpeed()
    {
        if(slowOrFast == 0) { animator.speed = .55f; slowOrFast++; }
        else { animator.speed = .85f; }
        
    }
}
