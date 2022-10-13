using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;

public class runeSelector : MonoBehaviour
{
    spellSelector selector;
    public char[] runes;
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

    // Start is called before the first frame update
    void Start()
    {   
        selector = GetComponent<spellSelector>();
        runes = new char[6];
        runes[0] = 'X';
        runeCount = 0;
        disableRuneIcons();

        FireIcon = GameObject.Find("FireIcon");
        FireIcon.SetActive(false);
        WaterIcon = GameObject.Find("WaterIcon");
        WaterIcon.SetActive(false);
        EarthIcon = GameObject.Find("EarthIcon");
        EarthIcon.SetActive(false);
        AirIcon = GameObject.Find("AirIcon");
        AirIcon.SetActive(false);
        LightningIcon = GameObject.Find("LightningIcon");

        list = new dLRS(Lightning);
        dLRSNode water = new dLRSNode(Water);
        water.prev = list.head;
        list.head.next = water;
        dLRSNode earth = new dLRSNode(Earth);
        earth.prev = water;
        water.next = earth;
        dLRSNode fire = new dLRSNode(Fire);
        fire.prev = earth;
        earth.next = fire;
        dLRSNode air = new dLRSNode(Air);
        air.prev = fire;
        fire.next = air;
        air.next = list.head;
        list.head.prev = air;

        
    }

    void Update()
    {   
        
        Vector2 pos = new Vector2(0, 0);
        pos.y += Input.mouseScrollDelta.y * 1.0f;
        if (pos.y > 0 || pos.y < 0) { switchIcon(pos.y > 0); }
        
        if (Input.GetMouseButtonDown(0)) { lauchSpell(); }

    }

    public void switchIcon(bool next)
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

    public void disableRuneIcons()
    {  
            for (int j = 1; j < 6; j++)
            {
                GameObject currentRunes = GameObject.Find("Runes" + j);
                for (int i = 1; i < 7; i++)
                    currentRunes.transform.GetChild(i - 1).gameObject.SetActive(false);
            }
            runeCount = 0;
    }

    public void setIcon(char type)
    {   
        if (type == 'L') { LightningIcon.SetActive(true); }
        else if (type == 'A') { AirIcon.SetActive(true); }
        else if (type == 'E') { EarthIcon.SetActive(true); }
        else if (type == 'W') { WaterIcon.SetActive(true); }
        else if (type == 'F') { FireIcon.SetActive(true); }
    }

    public void disableIcon(char type)
    {
        if (type == 'L') { LightningIcon.SetActive(false); }
        else if (type == 'A') { AirIcon.SetActive(false); }
        else if (type == 'E') { EarthIcon.SetActive(false); }
        else if (type == 'W') { WaterIcon.SetActive(false); }
        else if (type == 'F') { FireIcon.SetActive(false); }
    }

    public void AddRune()
    {
        int runeGroup = 0;
        if (runeCount >= 6)
            return;
        char type = list.getData();
        if (type == 'F') { runeGroup = 1; }
        else if (type == 'E') { runeGroup = 2; }
        else if (type == 'W') { runeGroup = 3; }
        else if (type == 'A') { runeGroup = 4; }
        else if (type == 'L') { runeGroup = 5; }

        GameObject currentRunes = GameObject.Find("Runes" + runeGroup);
        currentRunes.transform.GetChild(runeCount).gameObject.SetActive(true);
        runes[runeCount] = list.getData();
        runeCount++;
    }

    public void lauchSpell()
    {
        if (runes[0] == 'X')
            return;

        Vector2 loc = new Vector2(0, 0);
        loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selector.selectSpell(runes, loc);

        for(int i = 0; i < runes.Length; i++) { runes[i] = 'X'; };
        disableRuneIcons();
      
    }
}

    
