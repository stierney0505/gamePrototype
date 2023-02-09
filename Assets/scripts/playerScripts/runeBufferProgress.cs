using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class runeBufferProgress : MonoBehaviour
{ 
    private static float progress; //Progress is the progress to the next rune,
    private static int total = 100, //total is the total amount the progress int needs to be at to gain a rune
        runeAmount; //runeAmount is the amount of runes the player has
    private static GameObject[] progressBars; //an array of the progress bars in the rune buffer
                                              // Start is called before the first frame update
    
    void Start()
    {
        progress = 0;
        runeAmount= 0;
        GameObject parentBar = GameObject.Find("ProgressBars");
        progressBars = new GameObject[5];
        for(int i = 0; i < 5; i++) 
            progressBars[i] = transform.GetChild(i).gameObject;

        changeProgressBarColor(dLRSNode.types.FIRE);//currently the first element the character has is fire so this sets the progress bar to fire
    }

    public static void incrementProgress(float value) //This increments the progress bar
    {
        if (runeAmount == 4) //if there is 5 runes no more progress can be made on the rune buffer
            return;
        progress += value; //Increments the value of progress
        if(progress > total) //If progress has surpassed the total for the bar
        {   
            progressBars[runeAmount].GetComponent<Image>().fillAmount = 1;
            progress -= total; 
            runeAmount++;
            runeSelector.AddRune();
        }
        else
            progressBars[runeAmount].GetComponent<Image>().fillAmount = progress/total; //Increases the visible progress on the progress bar
    }

    public static void changeProgressBarColor(dLRSNode.types type) //This method just changes the color of the current progress bar based on the parameter
    {
        Color newColor;
        switch(type) //Switch statement that takes the type parameter and changes the color based on it
        {
            case dLRSNode.types.FIRE:
                newColor = new Color(.812f, .341f, .235f);
                break;
            case dLRSNode.types.EARTH:
                newColor = new Color(.478f, .282f, .255f);
                break;
            case dLRSNode.types.WATER:
                newColor = new Color(.122f, .494f, 1);
                break;
            case dLRSNode.types.AIR:
                newColor = new Color(.812f, .341f, .235f);
                break;
            case dLRSNode.types.LIGHTNING:
                newColor = new Color(1, .843f, .514f);
                break;
            case dLRSNode.types.DARK:
                newColor = new Color(.486f, .071f, .490f);
                break;
            default:
                newColor = new Color(255, 255, 255);
                break;
        }
        progressBars[runeAmount].GetComponent<Image>().color = newColor;
    }

    public static void setRuneAmount(int amount) //This method serves to set the value of the runeAmount
    {
        runeAmount = amount;
    }

    public static void resetBarProgress()
    {
        runeAmount = 0;
        for (int i = 0; i < 5; i++)
            progressBars[runeAmount].GetComponent<Image>().fillAmount = 0; 
    }
}
