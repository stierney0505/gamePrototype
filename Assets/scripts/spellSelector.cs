using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class spellSelector : MonoBehaviour
{
    
    public string spellSelect(char[] runes, dLRS list) //This method shound return the string of the rune type
    {                                                  //that has the most runes on the rune chart
        int Tcount = 0;
        int[] runeCount = {0,0,0,0,0}; //this array counts the number of each rune, Lightning being 1, fire being 2 and so on and so forth

        for(int i = 0; i < runes.Length; i++)
        {
            switch (runes[i])//This checks what runes[i] is and increments the runecount for that type
            {
                case 'L':
                    runeCount[0]++; Tcount++;
                    break;
                case 'F':
                    runeCount[1]++; Tcount++;
                    break;
                case 'A':
                    runeCount[2]++; Tcount++;
                    break;
                case 'E':
                    runeCount[3]++; Tcount++;
                    break;
                case 'W':
                    runeCount[4]++; Tcount++;
                    break;
            }
        }
        return highestCount(runeCount, list) + (Tcount + 1);   
    }

    public string highestCount(int[] runeCount, dLRS list) //This method returns the string of the type of rune that has the highest count
    {                                                      //If there is an equally high amount of runes then it chooses a random rune
        int[] checkArr = { 0, 0, 0, 0, 0 };
        if(Enumerable.SequenceEqual(checkArr, runeCount)) { return convertIndexToString(list.getId()); }

        int indexOfMax = -1; 
        int indexOf2nd = -1;
        int indexOf3rd = -1;
        int max = 0;

        
        for (int i = 0; i < runeCount.Length; i++) //this just iterates through the rune buffer and determines which rune appears the most, 2nd most, and 3rd most
        {
            if (runeCount[i] > max) { indexOfMax = i; max = runeCount[i]; indexOf2nd = -1; indexOf3rd = -1; }
            else if (runeCount[i] == max && runeCount[i] != 0) { indexOf2nd = i; }
            else if (runeCount[i] == max && runeCount[i] != 0 && indexOfMax == indexOf2nd) { indexOf3rd = i; }
        }

        if(indexOf2nd == -1) { return convertIndexToString(indexOfMax); } 
        else if(indexOf2nd != -1 && indexOf3rd == -1){
            int rand = (int)Random.Range(1, 3);
            if (rand == 1) {return convertIndexToString(indexOfMax); }
            else if(rand == 2) { return convertIndexToString(indexOf2nd); }
        }
        else if(indexOf3rd != -1) {
            int rand = (int)Random.Range(1, 4);
            if (rand == 1) { return convertIndexToString(indexOfMax); }
            else if (rand == 2) { return convertIndexToString(indexOf2nd); }
            else if (rand == 3) { return convertIndexToString(indexOf3rd); }
        }
        return null; //if this returns I have messed up
    }

    public string convertIndexToString(int index) //this helper methods converts the index from the rune array
    {                                             //to a string of the rune type based on the criteria on line 12 for the rune count array
        switch (index)
        {
            case 0:
                return "lightning";
            case 1:
                return "fire";
            case 2:
                return "air";
            case 3:
                return "earth";
            case 4:
                return "water";
            default:
                return null;//if this returns I indeed have messed up
        }
    }

}
