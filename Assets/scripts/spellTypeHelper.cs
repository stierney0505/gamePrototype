using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class spellTypeHelper 
{
    public static string getOnHitEffect(char type) { 

        int rand = Random.Range(1, 3);
        switch (type)
        {
            case 'L':
                return "HitEffects/lightning" + rand;
            case 'F':
                return "HitEffects/fire" + rand;
            case 'E':
                return "HitEffects/earth" + rand;
            case 'A':
                return "HitEffects/air" + rand;
            case 'W':
                return "HitEffects/water" + rand;
            case 'D':
                return "HitEffects/dark" + rand;
            default:
                return null;
        }
    }

}
