using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRS //Doubly Linked-List Rune Selector Class, pretty simple it will hold 5 chars for the rune types
{                 // F for fire, W for water, L for lightning, E for earth, A for air
    public dLRSNode head;

    public dLRS(char type)
    {
        head = new dLRSNode(type);
        
    }
    public char getData() { return head.type; }
    public void next() { head = head.next; }
    public void prev() { head = head.prev; }

    public static dLRS createList(char[] types)
    {
        dLRS typeArray;


        typeArray = new dLRS(types[0]); 
        dLRSNode type2 = new dLRSNode(types[1]);
        type2.prev = typeArray.head; 
        type2.next = typeArray.head;
        typeArray.head.next = type2;
        typeArray.head.prev = type2;
        

        return typeArray;
    }
}



