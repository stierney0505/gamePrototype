using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class spellSelector : MonoBehaviour
{
   
    public void selectSpell(char[] runes, Vector2 loc, dLRS list) //This method takes the runes from the buffer and based upon them
    {                                                               //Fires a spell prefab
        string spellName = "Spells/" + spellSelect(runes, list);
        GameObject spell = Instantiate(Resources.Load(spellName)) as GameObject;
        spell.transform.position = loc;
        spell.SetActive(true);
    }

    public string spellSelect(char[] runes, dLRS list) //This method shound return the string of the rune type
    {                                                  //that has the most runes on the rune chart
        int Tcount = 0;
        int[] runeCount = {0,0,0,0,0};

        for(int i = 0; i < runes.Length; i++)
        {
            if(runes[i] == 'L') { runeCount[0]++; Tcount++; }
            else if (runes[i] == 'F') { runeCount[1]++; Tcount++; }
            else if (runes[i] == 'A') { runeCount[2]++; Tcount++; }
            else if (runes[i] == 'E') { runeCount[3]++; Tcount++; }
            else if (runes[i] == 'W') { runeCount[4]++; Tcount++; }
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

        
        for (int i = 0; i < runeCount.Length; i++)
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
    {                                             //to a string of the rune type
        if(index == 0) { return "lightning"; }
        else if (index == 1) { return "fire"; }
        else if (index == 2) { return "air"; }
        else if (index == 3) { return "earth"; }
        else if (index == 4) { return "water"; }
        return null; //if this returns I indeed have messed up
    }
}
