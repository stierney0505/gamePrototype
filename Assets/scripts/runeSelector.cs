using UnityEngine;

public class runeSelector : MonoBehaviour //This class manages the rune selection by the player
{
    spellSelector selector;
    private char[] runes;
    private int runeCount;
    private GameObject FireIcon;
    private GameObject AirIcon;
    private GameObject LightningIcon;
    private GameObject WaterIcon;
    private GameObject EarthIcon;
    private const char Lightning = 'L';
    private const char Earth = 'E';
    private const char Fire = 'F';
    private const char Water = 'W';
    private const char Air = 'A';
    public dLRS list;
    private bool locked = false, chargeActive = false; 
    Vector2 mousePos;
    string spellName;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        selector = GetComponent<spellSelector>(); //Set up for rune buffer
        runes = new char[5];
        runes[0] = 'X';
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
        LightningIcon = GameObject.Find("LightningIcon");

        list = new dLRS(Lightning, 0); //This code block creates a circular doubly linked list for the rune selector
        dLRSNode water = new dLRSNode(Water, 4);//I thought about putting it into a method but that isn't necessary
        water.prev = list.head; //at this point of the project as I only need the list created on the startup of the rune selector
        list.head.next = water; 
        dLRSNode earth = new dLRSNode(Earth, 3);
        earth.prev = water;
        water.next = earth;
        dLRSNode fire = new dLRSNode(Fire, 1);
        fire.prev = earth;
        earth.next = fire;
        dLRSNode air = new dLRSNode(Air, 2);
        air.prev = fire;
        fire.next = air;
        air.next = list.head;
        list.head.prev = air; //End of circular list creation

        
    }

    void Update()
    {
        if (!locked)
        {
            Vector2 pos = new Vector2(0, 0); //This block of code just checks if the player clicked or scrolled
            pos.y += Input.mouseScrollDelta.y * 1.0f; //if they scrolled switches rune, if they clicked they lauch a spell
            if (pos.y > 0 || pos.y < 0) { switchIcon(pos.y > 0); }
            if (Input.GetMouseButtonDown(0)) { mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(runes, list);
                if (runes[0] != 'X') { animator.SetTrigger(getAttackType(spellName[0])); }
                else { animator.SetTrigger(getAttackType(list.getData())); }
            }
        }
    }

    public void switchIcon(bool next)//This just switches between icons based off the parameter
    {
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
            for (int j = 1; j < 6; j++)
            {
                GameObject currentRunes = GameObject.Find("Runes" + j);
                for (int i = 1; i < 6; i++)
                    currentRunes.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
            runeCount = 0;
    }

    public void setIcon(char type) //Currently I have my gui elements all overlaping and disable them and enable them as need be, on the backlog for refactoring but it ain't broken right now so I don't need to fix it
    {
        switch (type)
        {
            case 'F':
                FireIcon.SetActive(true);
                break;
            case 'E':
                EarthIcon.SetActive(true);
                break;
            case 'W':
                WaterIcon.SetActive(true);
                break;
            case 'A':
                AirIcon.SetActive(true);
                break;
            case 'L':
                LightningIcon.SetActive(true);
                break;
        }
    }

    public void disableIcon(char type)
    {
        switch (type)
        {
            case 'F':
                FireIcon.SetActive(false);
                break;
            case 'E':
                EarthIcon.SetActive(false);
                break;
            case 'W':
                WaterIcon.SetActive(false);
                break;
            case 'A':
                AirIcon.SetActive(false);
                break;
            case 'L':
                LightningIcon.SetActive(false);
                break;
        }
    }

    public void AddRune() //This method adds a rune to the rune buffer based on what rune is selected in the rune selector
    {
        int runeGroup = 0;
        if (runeCount >= 5)
            return;
        char type = list.getData();
        switch (type)
        {
            case 'F':
                runeGroup = 1;
                break;
            case 'E':
                runeGroup = 2;
                break;
            case 'W':
                runeGroup = 3;
                break;
            case 'A':
                runeGroup = 4;
                break;
            case 'L':
                runeGroup = 5;
                break;
        }

        GameObject currentRunes = GameObject.Find("Runes" + runeGroup);
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true);
        runes[runeCount] = list.getData();
        runeCount++;
    }

    public void AddRune(char rune) //This method adds a rune to the rune buffer based on what rune is selected in the rune selector
    {
        int runeGroup = 0;
        if (runeCount >= 5)
            return;
        char type = rune;

        switch (type)
        {
            case 'F' :
                runeGroup = 1;
                break;
            case 'E':
                runeGroup = 2;
                break;
            case 'W':
                runeGroup = 3;
                break;
            case 'A':
                runeGroup = 4;
                break;
            case 'L':
                runeGroup = 5;
                break;
        }
        GameObject currentRunes = GameObject.Find("Runes" + runeGroup);
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true);
        runes[runeCount] = list.getData();
        runeCount++;
    }

    public void lauchSpell() //This method current triggers the spellSelector's lauch spell method and give it the camera
    {                        //Position of where the player clicked

        if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); }
        else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); };
        createSpell(spellName, mousePos, list);
        for(int i = 0; i < runes.Length; i++) { runes[i] = 'X'; };
        disableRuneIcons();
      
    }

    public void createSpell(string name, Vector2 loc, dLRS list) //This method takes the runes from the buffer and based upon them
    {                                                           //Fires a spell prefab based on parameters
        string spellName = "Spells/" + name;
        GameObject spell = Instantiate(Resources.Load(spellName)) as GameObject;
        spell.transform.position = loc;
        spell.SetActive(true);

    }

    public void switchLocked() { locked = !locked; chargeActive = !chargeActive; }
    public void unlock() { locked = false; } //This method just 'unlocks' the rune selector and is called on the first run frame so that the player can quickly cancel the charge if they are doing so
    public bool isChargeActive() { return chargeActive; }
    public string getAttackType(char type) //This helper method takes a char and returns the animation trigger based upon the char type 
    {
        switch (type)
        {
            case 'F':
            case 'f':
                return "fAttack";
            case 'E':
            case 'e':
                return "eAttack";
            case 'W':
            case 'w':
                return "wAttack";
            case 'A':
            case 'a':
                return "aAttack";
            case 'L':
            case 'l':
                return "lAttack";
            default:
                return null;
        }
    }

}


