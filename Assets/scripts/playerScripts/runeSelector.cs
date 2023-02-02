using UnityEditorInternal;
using UnityEngine;

public class runeSelector : MonoBehaviour //This class manages the rune selection by the player
{
    spellSelector selector;
    PlayerScript pScript;
    Animator animator;
    private GameObject FireIcon, EarthIcon, AirIcon, LightningIcon, DarkIcon, WaterIcon, runeIcons;
    float runeSwitchCD = 1; //The cooldown between rune switches
    private bool switchReady = true, barActive = false; //switchActive bool is for checking if the character is able to switch runes
    // Start is called before the first frame update
    void Start()
    {
        selector = GetComponent<spellSelector>();
        animator = GetComponent<Animator>();
        pScript = GetComponent<PlayerScript>();
        runeIcons = GameObject.Find("Spell Slots");

        disableRuneIcons();

        FireIcon = GameObject.Find("FireIcon"); //This block of code disables the icons on the gui on start,
        FireIcon.SetActive(false);              
        WaterIcon = GameObject.Find("WaterIcon"); 
        WaterIcon.SetActive(false);
        EarthIcon = GameObject.Find("EarthIcon");
        EarthIcon.SetActive(false);
        AirIcon = GameObject.Find("AirIcon");
        AirIcon.SetActive(false);
        DarkIcon = GameObject.Find("DarkIcon");
        DarkIcon.SetActive(false);
        LightningIcon = GameObject.Find("LightningIcon");
        LightningIcon.SetActive(false);

    }

    void Update()
    {
        if (!switchReady && runeSwitchCD < 1f) //This if statement checks if the player has just switched(switchReady == false)
        {                                      // and if they have switched within 1 second (runeSwitchCD)
            switchReady = false; 
            runeSwitchCD += (Time.deltaTime);
        }
        else if (!switchReady && runeSwitchCD >= 1f) //If the player has switched greater than 1 second ago, 
            switchReady = true; //Changes the switchReady bool to true        

        Vector2 pos = new Vector2(0, 0); //This block of code just checks if the player clicked or scrolled
        pos.y += Input.mouseScrollDelta.y * 1.0f; //if they scrolled switches rune, if they clicked they lauch a spell
        if (pos.y > 0 || pos.y < 0)
        {
            if (switchReady && !barActive)
            {
                switchIcon(pos.y > 0);
                animator.SetLayerWeight(pScript.getLayer(), 1);
            }
        }
    }

    public void switchIcon(bool next)//This just switches between icons based off the parameter
    {
        runeSwitchCD = 0f;
        switchReady = false;
        disableIcon(pScript.getList().getData());
        if (next)
        {
            pScript.getList().next();
            setIcon(pScript.getList().getData());
        }
        else
        {
            pScript.getList().prev();
            setIcon(pScript.getList().getData());
        }
    }

    public void disableRuneIcons() //this removes the rune icons in the rune buffer
    {  
            for (int j = 1; j < 7; j++)
            {
                GameObject currentRunes = runeIcons.transform.Find("Runes" + j).gameObject;
                for (int i = 1; i < 6; i++)
                    currentRunes.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
            pScript.setRuneCount(0);
    }

    public void setIcon(dLRSNode.types type) //Currently I have my gui elements all overlaping and disable them and enable them as need be, on the backlog for refactoring but it ain't broken right now so I don't need to fix it
    {
        switch (type)
        {
            case dLRSNode.types.FIRE:
                FireIcon.SetActive(true);
                break;
            case dLRSNode.types.EARTH:
                EarthIcon.SetActive(true);
                break;
            case dLRSNode.types.WATER:
                WaterIcon.SetActive(true);
                break;
            case dLRSNode.types.AIR:
                AirIcon.SetActive(true);
                break;
            case dLRSNode.types.LIGHTNING:
                LightningIcon.SetActive(true);
                break;
            case dLRSNode.types.DARK:
                DarkIcon.SetActive(true);
                break;
        }
    }

    public void disableIcon(dLRSNode.types type)
    {
        switch (type)
        {
            case dLRSNode.types.FIRE:
                FireIcon.SetActive(false);
                break;
            case dLRSNode.types.EARTH:
                EarthIcon.SetActive(false);
                break;
            case dLRSNode.types.WATER:
                WaterIcon.SetActive(false);
                break;
            case dLRSNode.types.AIR:
                AirIcon.SetActive(false);
                break;
            case dLRSNode.types.LIGHTNING:
                LightningIcon.SetActive(false);
                break;
            case dLRSNode.types.DARK:
                DarkIcon.SetActive(false);
                break;
        }
    }

    public void AddRune() //This method adds a rune to the rune buffer based on what rune is selected in the rune selector, made to be called from the animator
    {
        int runeGroup = 0; //This int is used for finding the correct rune group in the GUI
        int runeCount = pScript.getRuneCount(); //These statments get the values from the Playscript
        dLRSNode.types[] runes = pScript.getRunes();
        dLRSNode.types nextSpellType = pScript.getNextSpellType();
        dLRS list = pScript.getList();

        if (runeCount >= 5) //If the player has 5 runes they shouldn't be able to get any more
            return;
        dLRSNode.types type = pScript.getList().getData();
        switch (type) //This switch statement takes the rune type and finds the gui group related to the element, and changes runeGroup to match that group
        {
            case dLRSNode.types.FIRE: //TODO REPLACE int runegroup with a string for better readability and rename gui elements
                runeGroup = 1;
                break;
            case dLRSNode.types.EARTH:
                runeGroup = 2;
                break;
            case dLRSNode.types.WATER:
                runeGroup = 3;
                break;
            case dLRSNode.types.AIR:
                runeGroup = 4;
                break;
            case dLRSNode.types.LIGHTNING:
                runeGroup = 5;
                break;
            case dLRSNode.types.DARK:
                runeGroup = 6;
                break;
        }

        GameObject currentRunes = runeIcons.transform.Find("Runes" + runeGroup).gameObject; //These two statements get the gui Element and 
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true); //make the gui element active to indicate a rune in the rune buffer
        runes[runeCount] = list.getData();
        runeCount++;
        nextSpellType = runes[runeCount -1];
        nextSpellType = selector.updateNextSpell(runes, nextSpellType, runeCount);

        pScript.setRunes(runes); //These statements update the PlayerScript's value in order to maintain
        pScript.setNextSpellType(nextSpellType); //the rune-value coherency
        pScript.setRuneCount(runeCount);
    }

    public void AddRune(dLRSNode.types rune) //This overload of the method adds a rune to the rune buffer based on what rune is selected in the rune selector,
    {

        int runeGroup = 0; //This int is used for finding the correct rune group in the GUI
        int runeCount = pScript.getRuneCount(); //These statments get the values from the Playscript
        dLRSNode.types[] runes = pScript.getRunes();
        dLRSNode.types nextSpellType = pScript.getNextSpellType();

        if (runeCount >= 5)
            return;
        dLRSNode.types type = rune;

        switch (rune)
        {
            case dLRSNode.types.FIRE:
                runeGroup = 1;
                break;
            case dLRSNode.types.EARTH:
                runeGroup = 2;
                break;
            case dLRSNode.types.WATER:
                runeGroup = 3;
                break;
            case dLRSNode.types.AIR:
                runeGroup = 4;
                break;
            case dLRSNode.types.LIGHTNING:
                runeGroup = 5;
                break;
            case dLRSNode.types.DARK:
                runeGroup = 6;
                break;
        }

        GameObject currentRunes = runeIcons.transform.Find("Runes" + runeGroup).gameObject; //These two statements get the gui Element and 
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true); //make the gui element active to indicate a rune in the rune buffer
        runes[runeCount] = rune;
        runeCount++;
        nextSpellType = rune;
        nextSpellType = selector.updateNextSpell(runes, nextSpellType, runeCount);


        pScript.setRunes(runes); //These statements update the PlayerScript's value in order to maintain
        pScript.setNextSpellType(nextSpellType); //the rune-value coherency
        pScript.setRuneCount(runeCount);
    }

    public void setBarBool(bool isActive) { barActive = isActive; }
}   


