using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sequenceNode : Node //This is the sequence node class and it requires that it has leafNode children to execute a sequence, similar where each node needs to 
{                                //be completed in order for it to return SUCCESS, like an AND statement
    public sequenceNode() { }

    public sequenceNode(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();
        if (childStatus == Status.RUNNING)
            return Status.RUNNING; //Returns running because this only reaches this statement if childStatus is Running
        else if (childStatus == Status.FAILURE)
            return Status.FAILURE; //Returns Failure because this only reaches this statemment if childStatus is failure

        currentChild++;
        if (currentChild >= children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS; //At this point the childStatus should be Success so it returns childStatus
        }
        return Status.RUNNING;
    }
}