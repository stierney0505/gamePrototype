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

    //This method takes the attacking type, the defending type, and the intial damage and applies a modifier based on what type
    // on what type is attacking and what type is defending. As of now, a 1.2x modifier for an attacking type that beats the defending type
    // and a .8x modifier for a attacking type that gets beaten by the defending type. Returns the original damage if there is no stronger type
    public static float damageModifier(char atkType, char defType, float dmg) 
    {
        float returnDmg = dmg;

        switch (atkType, defType)
        {
            //These statements are for when the attacking type is stronger than the defending type
            case ('F', 'A'): //Fire beats air so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('A', 'E'): //Air beats earth so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('E', 'L'): //Earth beats lightning so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('L', 'W'): //Lightning beats water so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('W', 'F'): //Water beats fire so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;

                //These statements are for a when the attacking type is weaker than the defending type
            case ('A', 'F'): //Air loses to fire so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('E', 'A'): //Earth loses to Air so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('L', 'E'): //Lightning loses to earth so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('W', 'L'): //Water loses to lightning so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case ('F', 'W'): //Fires loses to water so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
        }

        return returnDmg;
    }

}
