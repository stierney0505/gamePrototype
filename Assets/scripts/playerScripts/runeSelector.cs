using UnityEditorInternal;
using UnityEngine;

public class runeSelector : MonoBehaviour //This class manages the rune selection by the player
{
    spellSelector selector;
    private dLRSNode.types[] runes;
    private int runeCount;
    private GameObject FireIcon, EarthIcon, AirIcon, LightningIcon, DarkIcon, WaterIcon;
    public dLRS list;
    private bool locked = false, chargeActive = false, altFire = false, switchActive = true, barActive = false; 
    Vector2 mousePos;
    string spellName;
    Animator animator;
    dLRSNode.types nextSpellType;
    float runeSwitchCD = 1;

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


        dLRSNode.types[] elements = new dLRSNode.types[2];
        switch (gameObject.name[0]) //Each character gets only 2 elements to charge/use basic attacks with, so the first letter of the character is either W, R, or B 
        {                           //For the white, red, and blue witch respectively and each will get two elements based upon that.
            case 'W':
                elements[0] = dLRSNode.types.WATER;
                elements[1] = dLRSNode.types.AIR;
                setIcon(dLRSNode.types.WATER);
                break;

            case 'R':
                elements[0] = dLRSNode.types.EARTH;
                elements[1] = dLRSNode.types.FIRE;
                setIcon(dLRSNode.types.EARTH);
                break;

            case 'B':
                elements[0] = dLRSNode.types.LIGHTNING;
                elements[1] = dLRSNode.types.DARK;
                setIcon(dLRSNode.types.LIGHTNING);
                break;
        }

        list = dLRS.createList(elements);
    }

    void Update()
    {

        if (switchActive) { }
        else if (runeSwitchCD < 1f)
        {
            switchActive = false;
            runeSwitchCD += (Time.deltaTime);
        }
        else if (runeSwitchCD >= 1f)
            switchActive = true;

        if (!locked)
        {
            Vector2 pos = new Vector2(0, 0); //This block of code just checks if the player clicked or scrolled
            pos.y += Input.mouseScrollDelta.y * 1.0f; //if they scrolled switches rune, if they clicked they lauch a spell
            if (switchActive && !barActive && (pos.y > 0 || pos.y < 0)) { switchIcon(pos.y > 0); }

            if (Input.GetMouseButtonDown(1) && runeCount != 0)
            {
                altFire = true;
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(nextSpellType, runeCount);
                selector.createSpellCircle(nextSpellType, true, runeCount, false);
                animator.SetTrigger(getAttackType(nextSpellType));
                if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); } //rotates the player towards the direction they clicked
                else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            }

            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(list.getData());
                selector.createSpellCircle(list.getData(), false, runeCount, false);
                animator.SetTrigger(getAttackType(list.getData()));
                if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); } //rotates the player towards the direction they clicked
                else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            }

        }
        
    }

    public void switchIcon(bool next)//This just switches between icons based off the parameter
    {
        runeSwitchCD = 0f;
        switchActive = false;
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
    public string getAttackType(dLRSNode.types type) //This helper method takes a 'types' enum and returns the animation trigger based upon the enum type 
    {
        switch (type)
        {
            case dLRSNode.types.FIRE:
                return "fAttack";
            case dLRSNode.types.EARTH:
                return "eAttack";
            case dLRSNode.types.WATER:
            case dLRSNode.types.ICE:
                return "wAttack";
            case dLRSNode.types.AIR:
                return "aAttack";
            case dLRSNode.types.LIGHTNING:
                return "lAttack";
            case dLRSNode.types.DARK:
                return "dAttack";
            default:
                return null;
        }
    }

    internal void createChargeCircle()
    {
       selector.createSpellCircle(list.getData(), true, runeCount, true);
    }
    public void setBarBool(bool isActive) { barActive = isActive; }
}


