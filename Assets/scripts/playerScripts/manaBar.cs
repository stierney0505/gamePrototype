using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manaBar : MonoBehaviour
{
    private static float totalMana, //Total mana is the total mana the character can possess
        currentMana; //CurrentMana is the current mana the character has at any given moment
    private static Image healthImage;
    private static float manaRegenCD, manaRegen = 10; //ManaRegenCD is the counter for the cooldown before mana can regen and manaregen is the amount of mana
                                  //that gets regenerated
    static bool regenActive = false;//This bool checks if the mana is regening
    // Start is called before the first frame update
    void Start()
    {
        healthImage = GetComponent<Image>();
    }

    private void Update()
    {
        if (currentMana == totalMana) //If currentMana == totalMana there is no reason to perform mana update functions
            return;
        else if (regenActive)//If regenActive, mana will regen over time
            incrementPlayerMana(manaRegen * Time.deltaTime, false); 
        else if (manaRegenCD < 4) //If regenActive is false and manaRegenCoolDown is less than 4, increments manaregenCD
            manaRegenCD += Time.deltaTime;
        else if (manaRegenCD >= 4) //If regenActive is false and manaRegenCoolDown is greater or equal to 4, resets the CD and enables regen
        {   regenActive = true;
            manaRegenCD = 0;
        }
    }   

    public static void setManaBarValue(float value) { totalMana = value; currentMana = totalMana; }
    public static float getManaBarValue() { return currentMana; }
    public static void incrementPlayerMana(float value, bool remove)
    {
        currentMana += value;
        if (remove) {  //if the parameter for remove is true then mana is being taken away an so regenactive = false;
            regenActive = false; 
            manaRegenCD = 0; } //Reset manaRegenCD because mana was incremented
        else if(currentMana > totalMana) //If currentMana > totalMana the current mana needs to be set to totalMana
            currentMana = totalMana;
        healthImage.fillAmount = (currentMana / totalMana);
    }
}
