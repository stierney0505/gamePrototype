using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dLRSNode //Doubly Linked-List Rune Selector Node, to be used for the rune selection
{                     //Each rune is represented by a char for its type, i.e. A = air
    public char type;
    public dLRSNode next;//of rune is currently in the selector when a spell is fired without charged runes, will refactor in the future
    public dLRSNode prev;

    public dLRSNode(char type1)
    {
        type = type1;
        next = null;
        prev = null;
    }

}

