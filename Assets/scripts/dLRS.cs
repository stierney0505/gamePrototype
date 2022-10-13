using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRS 
{
    public dLRSNode head;
   

    public dLRS(char type)
    {
        head = new dLRSNode(type);
        
    }
    public char getData() { return head.type; }
    public void next() { head = head.next; }
    public void prev() { head = head.prev; }
}



