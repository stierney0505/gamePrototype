using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class spellSelector : MonoBehaviour
{
    GameObject nextSpellUI; 

    private void Start()
    {
        nextSpellUI = GameObject.Find("NextSpell");//This finds the UI element, 'NextSpell', which holds the UI icon for which spell will be next
        disableChildren();
    }

    public char updateNextSpell(char[] runes, char nextSpellType, int runeCount)//This method checks the rune buffer for the latest rune, or if there is a combination and update the next spell type for the next spell cast
    {   //It also checks for spell combinations, which happens when there is an equal amount of specific runes, i.e. X earth and X water makes a wood spell, lighting and fire makes acid, water and wind makes Ice.
        int count1 = 0;
        int count2 = 0;
        char type1 = 'X';
        char type2 = 'X';
        char nextSpell = nextSpellType;

        for (int i = 0; i < runeCount; i++)
        {
            if (type1 == 'X')
            {   type1 = runes[i];//This block here iteratively checks each rune in the runes array
                count1++; //and if there is more than two than there isnt a combination so it breaks
            }
            else if (type1 != runes[i] && type2 == 'X')
            {   type2 = runes[i];
                count2++;
            }
            else if (type2 != runes[i])
            {
                goto End; //If there is more than three types in the rune array then there is no need to check for combinations as there will be none
            }
        }

        if(count1 != count2) //If there is not an equal amount of runes there is no need to check which combination it is
            goto End;

        switch (type1, type2) 
        {
            case ('E', 'W'): //This case checks if the types are Earth and water or vice versa for wood spell
            case ('W', 'E'):
                nextSpell = 'P';
                break;
            case ('L', 'F'): //This case checks if the types are lightning and fire or vice versa for acid spell
            case ('F', 'L'):
                nextSpell = 'C';
                break;
            case ('A', 'W'): //This case checks if the types are water and air or vice versa for ice spell
            case ('W', 'A'):
                nextSpell = 'I';
                break;
        }

        End:
            disableChildren();
            nextSpellUI.transform.GetChild(getChildrenNum(nextSpell)).gameObject.SetActive(true);
            return nextSpell;
    }
    public string spellSelect(char nextSpell, int runeCount) //This method shound return the string of the next spell based on the nextSpell char
    {                     
        switch (nextSpell)//This checks what runes[i] is and increments the runecount for that type
        {
            case 'L': //Case L for lighting
                return "lightning" + runeCount;
            case 'F': //Case F for Fire
                return "fire" + runeCount;
            case 'A': //Case A for Air
                return "air" + runeCount;
            case 'E': //Case E for Earth
                return "earth" + runeCount;
            case 'W': //Case W for Water
                return "water" + runeCount;
            case 'D': //Case D for Dark
                return "dark" + runeCount;
            case 'C': //case C for aCid becuase a is taken
                return "acid" + runeCount;
            case 'P': //case P for wood because wood is from trees which is from Plants
                return "wood" + runeCount;
            case 'I': //case I for Ice
                return "ice" + runeCount;
            default:
                return null; //Todo replace this with a placeholder spell for errors

        }
    }

    public int getChildrenNum(char type) //This method takes the spell type and returns the child number for the nextSpellUI element
    {   
        switch (type)
        {
            case 'L':
                return 0;
            case 'F':
                return 1;
            case 'W':
                return 2;
            case 'A':
                return 3;
            case 'E':
                return 4;
            case 'D':
                return 5;
            case 'C':
                return 6;
            case 'P':
                return 7;
            case 'I':
                return 8;
            default:
                return -1;
        }
    }

    public void disableChildren()
    {
        int totalChildren = nextSpellUI.transform.childCount;
        for (int i = 0; i < totalChildren; i++)
        {
            nextSpellUI.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}
