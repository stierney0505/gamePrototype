using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRSNode
{
    public char type;
    public dLRSNode next;
    public dLRSNode prev;

    public dLRSNode(char type1)
    {
        type = type1;
        next = null;
        prev = null;
    }


    public void setNext(dLRSNode next1) { next = next1; }
    public void setPrev(dLRSNode prev1) { prev = prev1; }

    public dLRSNode getPrev() { return prev; }
    public dLRSNode getNext() { return next; }

    public char getType() { return type; }
    public void setType(char type1) { type = type1; }
}

