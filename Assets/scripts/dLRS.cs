using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRS //Doubly Linked-List Rune Selector Class, pretty simple
{
    public dLRSNode head;

    public dLRS(char type, int id)
    {
        head = new dLRSNode(type, id);
        
    }
    public char getData() { return head.type; }
    public int getId() { return head.id; }
    public void next() { head = head.next; }
    public void prev() { head = head.prev; }
}



