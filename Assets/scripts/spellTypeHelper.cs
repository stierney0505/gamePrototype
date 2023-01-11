using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class spellTypeHelper 
{
    public static string getOnHitEffect(dLRSNode.types type) { 

        int rand = Random.Range(1, 3);
        switch (type)
        {
            case dLRSNode.types.LIGHTNING:
                return "HitEffects/lightning" + rand;
            case dLRSNode.types.FIRE:
                return "HitEffects/fire" + rand;
            case dLRSNode.types.EARTH:
                return "HitEffects/earth" + rand;
            case dLRSNode.types.AIR:
                return "HitEffects/air" + rand;
            case dLRSNode.types.WATER:
                return "HitEffects/water" + rand;
            case dLRSNode.types.DARK:
                return "HitEffects/dark" + rand;
            default:
                return null;
        }
    }

    //This method takes the attacking type, the defending type, and the intial damage and applies a modifier based on what type
    // on what type is attacking and what type is defending. As of now, a 1.2x modifier for an attacking type that beats the defending type
    // and a .8x modifier for a attacking type that gets beaten by the defending type. Returns the original damage if there is no stronger type
    public static float damageModifier(dLRSNode.types atkType, dLRSNode.types defType, float dmg) 
    {
        float returnDmg = dmg;

        switch (atkType, defType)
        {
            //These statements are for when the attacking type is stronger than the defending type
            case (dLRSNode.types.FIRE, dLRSNode.types.AIR): //Fire beats air so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.AIR, dLRSNode.types.EARTH): //Air beats earth so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.EARTH, dLRSNode.types.LIGHTNING): //Earth beats lightning so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.LIGHTNING, dLRSNode.types.WATER): //Lightning beats water so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.WATER, dLRSNode.types.FIRE): //Water beats fire so its damage is modified by 1.2x
                returnDmg *= 1.2f;
                return returnDmg;

                //These statements are for a when the attacking type is weaker than the defending type
            case (dLRSNode.types.AIR, dLRSNode.types.FIRE): //Air loses to fire so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.EARTH, dLRSNode.types.AIR): //Earth loses to Air so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.LIGHTNING, dLRSNode.types.EARTH): //Lightning loses to earth so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.WATER, dLRSNode.types.LIGHTNING): //Water loses to lightning so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
            case (dLRSNode.types.FIRE, dLRSNode.types.WATER): //Fires loses to water so its damage is modified by .8x
                returnDmg *= 1.2f;
                return returnDmg;
        }

        return returnDmg;
    }

}
