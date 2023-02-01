using UnityEditorInternal;
using UnityEngine;

public class runeSelector : MonoBehaviour //This class manages the rune selection by the player
{
    spellSelector selector;
    private dLRSNode.types[] runes;
    private short runeCount, //runeCount is the count of the runes used in a spell
        comboCount, //ComboCount will be incremented up to three based upon how many times the player clicks during the combo window
        comboCheck = 0; //ComboCheck increments at the end of each comboAttack as a way to check when the combo is done
    private GameObject FireIcon, EarthIcon, AirIcon, LightningIcon, DarkIcon, WaterIcon;
    public dLRS list; //List of the runes the player has
    private bool locked = false, //Locked bool is for stopping the rune selector from switching runes
        chargeActive = false, //this bool keeps track of if the character is charging a rune into the rune buffer
        altFire = false,  //altFire bool is true when the player uses the empowered spell, and it lets launchspell know to empty the rune buffer
        switchReady = true, //switchActive bool is for checking if the character is able to switch runes
        barActive = false, //barActive bool is for checking if the character is using a barrier
        comboActive = false; //comboActice bool is for checking if the character is performing a combo
    Vector2 mousePos; //this vector2 keeps track of the mousePos that the player clicked on to
    string spellName;
    Animator animator;
    dLRSNode.types nextSpellType;
    float runeSwitchCD = 1; //The cooldown between rune switches

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        selector = GetComponent<spellSelector>(); //Set up for rune buffer
        runes = new dLRSNode.types[5];
        runes[0] = dLRSNode.types.EMPTY;
        runeCount = 0;
        disableRuneIcons();

        FireIcon = GameObject.Find("FireIcon"); //This block of code disables the icons on the gui on start,
        FireIcon.SetActive(false);              //TODO replace gui icons with prefabs to add and delete them
        WaterIcon = GameObject.Find("WaterIcon"); //Instead of enabling and disabling
        WaterIcon.SetActive(false);
        EarthIcon = GameObject.Find("EarthIcon");
        EarthIcon.SetActive(false);
        AirIcon = GameObject.Find("AirIcon");
        AirIcon.SetActive(false);
        DarkIcon = GameObject.Find("DarkIcon");
        DarkIcon.SetActive(false);
        LightningIcon = GameObject.Find("LightningIcon");
        LightningIcon.SetActive(false);


        dLRSNode.types[] elements = new dLRSNode.types[6];
        switch (gameObject.name[0]) //Each character gets only 2 elements to charge/use basic attacks with, so the first letter of the character is either W, R, or B 
        {                           //For the white, red, and blue witch respectively and each will get two elements based upon that.
            case 'N':
                elements[0] = dLRSNode.types.FIRE;
                elements[1] = dLRSNode.types.DARK;
                elements[2] = dLRSNode.types.EARTH;
                elements[3] = dLRSNode.types.WATER;
                elements[4] = dLRSNode.types.AIR;
                elements[5] = dLRSNode.types.LIGHTNING;
                setIcon(dLRSNode.types.FIRE);
                break; 
        }

        list = dLRS.createList(elements);

        animator.SetLayerWeight(getLayer(), 1);
    }

    void Update()
    {
        if (!switchReady && runeSwitchCD < 1f) 
        {
            switchReady = false;
            runeSwitchCD += (Time.deltaTime);
        }
        else if (!switchReady && runeSwitchCD >= 1f)
            switchReady = true;

        if (!locked)
        {
            Vector2 pos = new Vector2(0, 0); //This block of code just checks if the player clicked or scrolled
            pos.y += Input.mouseScrollDelta.y * 1.0f; //if they scrolled switches rune, if they clicked they lauch a spell
            if (pos.y > 0 || pos.y < 0)
            {
                if (switchReady && !barActive)
                {
                    switchIcon(pos.y > 0);
                    animator.SetLayerWeight(getLayer(), 1);
                }
            }

            if (Input.GetMouseButtonDown(1) && runeCount != 0)
            {
                altFire = true;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(nextSpellType, runeCount);
                animator.SetLayerWeight(getLayer(dLRSNode.toStr(nextSpellType)), 1);
                animator.SetTrigger("specialAttack");
                if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); } //rotates the player towards the direction they clicked
                else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if(comboActive == false) { comboCount = 1; comboActive = true; } //These line checks if the character is in the middle of a combo, and if not sets the combo bool to true
                else if(comboActive == true && comboCount < 3) { comboCount++; }

                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(list.getData());
                animator.SetBool("attack" + comboCount, true);
                if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); } //rotates the player towards the direction they clicked
                else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            }

        }
        
    }

    public void switchIcon(bool next)//This just switches between icons based off the parameter
    {
        runeSwitchCD = 0f;
        switchReady = false;
        disableIcon(list.getData());
        if (next)
        {
            list.next();
            setIcon(list.getData());
        }
        else
        {
            list.prev();
            setIcon(list.getData());
        }
    }

    public void disableRuneIcons() //this removes the rune icons in the rune buffer
    {  
            for (int j = 1; j < 7; j++)
            {
                GameObject currentRunes = GameObject.Find("Runes" + j);
                for (int i = 1; i < 6; i++)
                    currentRunes.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
            runeCount = 0;
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

    public void AddRune() //This method adds a rune to the rune buffer based on what rune is selected in the rune selector
    {
        int runeGroup = 0;
        if (runeCount >= 5)
            return;
        dLRSNode.types type = list.getData();
        switch (type)
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

        GameObject currentRunes = GameObject.Find("Runes" + runeGroup);
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true);
        runes[runeCount] = list.getData();
        runeCount++;
        nextSpellType = runes[runeCount -1];
        nextSpellType = selector.updateNextSpell(runes, nextSpellType, runeCount);
    }

    public void AddRune(dLRSNode.types rune) //This method adds a rune to the rune buffer based on what rune is selected in the rune selector
    {
        int runeGroup = 0;
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
        GameObject currentRunes = GameObject.Find("Runes" + runeGroup);
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true);
        runes[runeCount] = rune;
        runeCount++;
        nextSpellType = rune;
        nextSpellType = selector.updateNextSpell(runes, nextSpellType, runeCount);
    }

    public void launchSpell() //This method current triggers the spellSelector's launch spell method and give it the camera
    {                        //Position of where the player clicked. Called through the animator
        GameObject spell = Instantiate(Resources.Load("Spells/" + spellName)) as GameObject;
        spell.transform.position = mousePos;
        spell.SetActive(true);
        if (altFire)
        {
            for (int i = 0; i < runes.Length; i++) { runes[i] = dLRSNode.types.EMPTY; };
            disableRuneIcons();
            selector.disableChildren();
            altFire = false;
        }
    }
    public void switchLocked() { locked = !locked; chargeActive = !chargeActive; }
    public void setLocked(bool isLocked) { locked = isLocked; }
    public void unlock() { locked = false; } //This method just 'unlocks' the rune selector and is called on the first run frame so that the player can quickly cancel the charge if they are doing so
    public bool isChargeActive() { return chargeActive; }
    public void setBarBool(bool isActive) { barActive = isActive; }
    public int getLayer() //This method checks if the current rune equipped is the same type as the animator layer through the toStr method
    {                     //this method also makes each layer than isn't the layer of the selected equal to 0
        int returnInt = 0;
        for (int i = 0; i < animator.layerCount; i++)  
        {
            if (list.toStr() == animator.GetLayerName(i))
                returnInt = i;
            else
                animator.SetLayerWeight(i, 0); //TODO once the amount of runes are selectable, optimize the code so that it only turns off the x-1 selected runes

        }
        return returnInt;
    }
    public int getLayer(string goalLayer) //This is an overload of the getLayer() method, this has a parameter as a goal layer to set as the top layer
    {                     
        int returnInt = 0;
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (goalLayer == animator.GetLayerName(i)) //checks if the current layer is the goal layer
                returnInt = i;
            else
                animator.SetLayerWeight(i, 0); 

        }
        return returnInt;
    }
    public void resetCombo() { //This method resets the comboActive bool and comboCount int, called through the animator at the end of each combo animation
        comboCheck++;
        if (comboCount > comboCheck)//Since comboCount goes up to 3, if comboCheck is ever equal to comboCount then the comboShould end
            return;

        comboActive = false; comboCount = 0; comboCheck = 0;  
        animator.SetBool("attack1", false); animator.SetBool("attack2", false); animator.SetBool("attack3", false);} 
    public void resetAnimatorLayer() { animator.SetLayerWeight(getLayer(), 1); } //This method switches the animator layer back to the element selected in the rune buffer, called through the animator at the end of a special attack or every hurt/death animation
}


