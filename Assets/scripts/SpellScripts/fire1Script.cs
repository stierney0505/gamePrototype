using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire1Script : spell
{   
    private Animator animator;
    int loops = 0;

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

    public void loop()
    {
        if (loops < 7) { loops++; }
        else { animator.SetTrigger("fade"); }
    }
}


