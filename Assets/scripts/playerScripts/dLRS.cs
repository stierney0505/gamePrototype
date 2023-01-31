using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRS //Doubly Linked-List Rune Selector Class, pretty simple it will hold 5 chars for the rune types
{                 // F for fire, W for water, L for lightning, E for earth, A for air
    public dLRSNode head, tail; //In a doubly linked list there is no head or tail, these are just used for the creation of a doubly linked list, i.e. linking the head to the tail

    public dLRS(dLRSNode.types type)
    {
        head = new dLRSNode(type);
        tail = head;
    }
    public dLRSNode.types getData() { return head.getData(); }
    public void next() { head = head.next; }
    public void prev() { head = head.prev; }
    public dLRSNode getTail() { return tail; }

    public static dLRS createList(dLRSNode.types[] types)
    {
        dLRS typeArray;


        
        typeArray = new dLRS(types[0]);
        if (types.Length > 1)
        {
            
            for (int i = 1; i < types.Length; i++)
            {
                dLRSNode type2 = new dLRSNode(types[i]);
                type2.prev = typeArray.head;
                typeArray.head.next = type2;
                typeArray.next();
            }
            typeArray.getTail().prev = typeArray.head;
            typeArray.head.next = typeArray.getTail();
            typeArray.head = typeArray.tail;
        }
        return typeArray;
    }

    public string toStr()
    {
        switch (head.type)
        {
            case dLRSNode.types.FIRE:
                return "Fire";
            case dLRSNode.types.EARTH:
                return "Earth";
            case dLRSNode.types.WATER:
                return "Water";
            case dLRSNode.types.AIR:
                return "Air";
            case dLRSNode.types.LIGHTNING:
                return "Lightning";
            case dLRSNode.types.DARK:
                return "Dark";
            default:
                return null;
        }
    }
}



