using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectorNode : Node  //This is the selector node class and it requires that it has leafNode children to execute one of the leafs, it searches for 
{                                //one node to complete in order to return success.

    public selectorNode(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING)
            return Status.RUNNING;

        if (childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
