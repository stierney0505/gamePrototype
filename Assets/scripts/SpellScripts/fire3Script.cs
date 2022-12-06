using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class fire3Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] char type; //to decide the spell type
    public int spellCount, limit;
    public Vector2 startLoc;


    private void Start()
    {   
        if(spellCount == 1)
            startLoc = transform.position;
        int rand = Random.Range(-60, 61);
        transform.Rotate(0, 0, rand);    
    }
    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void remove() { Destroy(gameObject); }
    public void end() { }
    public float getDamage() { return damage; }
    public char getType() { return type; }

    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public Vector2 getNextLoc()
    {
        float xPos = startLoc.x;
        float yPos = startLoc.y;
        float xRand = Random.Range(0, 2.5f); //The idea here is to get a random value from 0 to 2.5f for the x and y
        float yRand = Random.Range(0, 2.5f); //coordinate of the next spell to be created
        int posOrNeg = Random.Range(0, 2);   //However the mean of -2.5f to 2.5f is 0 which means the on average the 
        if (posOrNeg == 0)                   //next position will probably be somewhat close to 0, so I took the range
            xRand = xRand * -1;              //from 0 to 2.5f instead and using a rand of 0-1 for a 50/50 if the value of 
        posOrNeg = Random.Range(0, 2);       //x is neg and y is neg this way the spread is on average further from 0
        if (posOrNeg == 0)
            yRand = yRand * -1;
        return new Vector2(xPos - xRand, yPos - yRand);
    }

    public string getName(string objName)
    {
        int indexNum = objName.IndexOfAny("123456".ToCharArray());
        return objName.Substring(0, indexNum + 1);
    }

    public void createNewSpell()
    {
        if (spellCount < limit + 1)
        {
            GameObject spell = Instantiate(Resources.Load("Spells/" + getName(gameObject.name)) as GameObject);
            fire3Script newSpellScript = spell.GetComponent<fire3Script>();
            newSpellScript.spellCount = 1 + spellCount;
            newSpellScript.startLoc = startLoc;
            spell.transform.position = getNextLoc();
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
