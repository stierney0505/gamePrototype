using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    public static string toStr(types type) //this static method returns a string based upon what enum was put into the parameter
    {                                      //primarily made to discern what animator layer to set to, but could be reused
        switch (type)
        {
            case dLRSNode.types.FIRE:
                return("Fire");
            case dLRSNode.types.EARTH:
                return ("Earth");
            case dLRSNode.types.WATER:
                return ("Water");
            case dLRSNode.types.AIR:
                return ("Air");
            case dLRSNode.types.LIGHTNING:
                return ("Lightning");
            case dLRSNode.types.DARK:
                return ("Dark");
            default:
                return ("YOU MESSED UP");
        }
    }
}

