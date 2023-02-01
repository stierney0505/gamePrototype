using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class spellSelector : MonoBehaviour
{
    GameObject nextSpellUI;
    Transform beneath;
    Transform chargeLoc;
    private void Start()
    {
        nextSpellUI = GameObject.Find("NextSpell");//This finds the UI element, 'NextSpell', which holds the UI icon for which spell will be next
        disableChildren();
        beneath = transform.GetChild(1);
        chargeLoc = transform.GetChild(2);
    }

    public dLRSNode.types updateNextSpell(dLRSNode.types[] runes, dLRSNode.types nextSpellType, int runeCount)//This method checks the rune buffer for the latest rune, or if there is a combination and update the next spell type for the next spell cast
    {   //It also checks for spell combinations, which happens when there is an equal amount of specific runes, i.e. X earth and X water makes a wood spell, lighting and fire makes acid, water and wind makes Ice.
        int count1 = 0;
        int count2 = 0;
        dLRSNode.types type1 = dLRSNode.types.EMPTY;
        dLRSNode.types type2 = dLRSNode.types.EMPTY;
        dLRSNode.types nextSpell = nextSpellType;

        for (int i = 0; i < runeCount; i++)
        {
            if (type1 == dLRSNode.types.EMPTY || runes[i] == type1)
            {   type1 = runes[i];//This block here iteratively checks each rune in the runes array
                count1++; //and if there is more than two than there isnt a combination so it breaks
            }
            else if (type1 != runes[i] && (type2 == dLRSNode.types.EMPTY || runes[i] == type2))
            {   type2 = runes[i];
                count2++;
            }
            else if (type2 != runes[i] && type1 != runes[i])
            {
                goto End; //If there is more than three types in the rune array then there is no need to check for combinations as there will be none
            }
        }

        if (count1 != count2)//If there is not an equal amount of runes there is no need to check which combination it is
            goto End;
        

        switch (type1, type2) 
        {
            case (dLRSNode.types.EARTH, dLRSNode.types.WATER): //This case checks if the types are Earth and water or vice versa for wood spell
            case (dLRSNode.types.WATER, dLRSNode.types.EARTH):
                nextSpell = dLRSNode.types.WOOD;
                break;
            case (dLRSNode.types.LIGHTNING, dLRSNode.types.FIRE): //This case checks if the types are lightning and fire or vice versa for acid spell
            case (dLRSNode.types.FIRE, dLRSNode.types.LIGHTNING):
                nextSpell = dLRSNode.types.ACID;
                break;
            case (dLRSNode.types.AIR, dLRSNode.types.WATER): //This case checks if the types are water and air or vice versa for ice spell
            case (dLRSNode.types.WATER, dLRSNode.types.AIR):
                nextSpell = dLRSNode.types.ICE;
                break;
        }

        End:
            disableChildren();
            nextSpellUI.transform.GetChild(getChildrenNum(nextSpell)).gameObject.SetActive(true);
            nextSpellType = nextSpell;
            return nextSpell;
    }
    public string spellSelect(dLRSNode.types nextSpell, int runes) //This method is for selecting spells using the spell buffer
    {
        int runeCount = runes + 1;
        switch (nextSpell)//This checks what runes[i] is and increments the runecount for that type
        {
            case dLRSNode.types.LIGHTNING: 
                return "lightning" + runeCount;
            case dLRSNode.types.FIRE: 
                return "fire" + runeCount;
            case dLRSNode.types.AIR: 
                return "air" + runeCount;
            case dLRSNode.types.EARTH: 
                return "earth" + runeCount;
            case dLRSNode.types.WATER: 
                return "water" + runeCount;
            case dLRSNode.types.DARK: 
                return "dark" + runeCount;
            case dLRSNode.types.ACID: 
                return "acid" + runeCount;
            case dLRSNode.types.WOOD: 
                return "wood" + runeCount;
            case dLRSNode.types.ICE: 
                return "ice" + runeCount;
            default:
                return null; //Todo replace this with a placeholder spell for errors
        }
    }

    public string spellSelect(dLRSNode.types nextSpell) //This method is for spells casted without the spell buffer using the runes
    {
        switch (nextSpell)//This checks what runes[i] is and increments the runecount for that type
        {
            case dLRSNode.types.LIGHTNING:
                return "lightning1";
            case dLRSNode.types.FIRE:
                return "fire1";
            case dLRSNode.types.AIR:
                return "air1";
            case dLRSNode.types.EARTH:
                return "earth1";
            case dLRSNode.types.WATER:
                return "water1";
            case dLRSNode.types.DARK:
                return "dark1";
            case dLRSNode.types.ACID:
                return "acid1";
            case dLRSNode.types.WOOD:
                return "wood1";
            case dLRSNode.types.ICE:
                return "ice1";
            default:
                return null; //Todo replace this with a placeholder spell for errors
        }
    }



    public int getChildrenNum(dLRSNode.types type) //This method takes the spell type and returns the child number for the nextSpellUI element
    {   
        switch (type)
        {
            case dLRSNode.types.LIGHTNING:
                return 0;
            case dLRSNode.types.FIRE:
                return 1;
            case dLRSNode.types.WATER:
                return 2;
            case dLRSNode.types.AIR:
                return 3;
            case dLRSNode.types.EARTH:
                return 4;
            case dLRSNode.types.DARK:
                return 5;
            case dLRSNode.types.ACID:
                return 6;
            case dLRSNode.types.WOOD:
                return 7;
            case dLRSNode.types.ICE:
                return 8;
            default:
                return -1;
        }
    }

    public void disableChildren() //This method disables the children of the NextSpell UI object
    {
        int totalChildren = nextSpellUI.transform.childCount;
        for (int i = 0; i < totalChildren; i++)
        {
            nextSpellUI.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}


