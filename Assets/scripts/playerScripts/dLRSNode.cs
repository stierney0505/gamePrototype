using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRSNode //Doubly Linked-List Rune Selector Node, to be used for the rune selection
{                     //Each rune is represented by the enum
    public enum types {DARK, FIRE, LIGHTNING, EARTH, WATER, AIR, WOOD, ACID, ICE, EMPTY};
    public types type;
    public dLRSNode next;//of rune is currently in the selector when a spell is fired without charged runes, will refactor in the future
    public dLRSNode prev;

    public dLRSNode(types newType)
    {
        type = newType;
        next = null;
        prev = null;
    }

    public types getData() { return type; }

}

