using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class spellTypeHelper 
{
    public static string getOnHitEffect(char type) { //

        int rand = Random.Range(1, 3);
        
        if (type == 'L') { return "HitEffects/lightning" + rand; }
        else if (type == 'F') { return  "HitEffects/fire" + rand; }
        else if (type == 'E') { return  "HitEffects/earth" + rand; }
        else if (type == 'A') { return  "HitEffects/air" + rand; }
        else if (type == 'W') { return  "HitEffects/water" + rand; }
        
        return null;
    }
    //TODO ad a method for creating on hit effects and knock back after I make the ai for enemies

}
