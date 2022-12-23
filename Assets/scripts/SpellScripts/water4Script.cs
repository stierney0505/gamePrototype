using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water4Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] char type; //to decide the spell type
    public int spellCount, limit;
    Vector2 nextLoc, direction;
    bool left; //if the spell is to the left of the player this is true


    private void Start()
    {
        
        if(spellCount ==  limit + 1) { knockBack *= 5; }
        if (spellCount == 0)
        {
            Vector2 startLoc, endLoc;
            spellCount = 1;
            endLoc = transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            startLoc = player.transform.GetChild(0).position;
            if (transform.position.x < player.transform.position.x)
            {
                transform.eulerAngles = new Vector2(0, 180);
                left = true;
                transform.position = new Vector2(startLoc.x - .15f, startLoc.y - .15f);
            }
            else
                transform.position = new Vector2(startLoc.x + .15f, startLoc.y - .15f);

            startLoc = transform.position;
            direction = (endLoc - startLoc).normalized; //Vector pointing from startLoc to endLoc
            getNextPoint();
        }
        else
        {  
            getNextPoint();
            if (left)
                transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void remove() { Destroy(gameObject); }
    public void end(bool environment)
    {
        if (environment)
            remove();
    }
    public float getDamage() { return damage; }
    public char getType() { return type; }

    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }

    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public string getName(string objName)
    {
        int indexNum = objName.IndexOfAny("123456".ToCharArray());
        return objName.Substring(0, indexNum + 1);
    }

    public void getNextPoint()
    {  
        nextLoc = (Vector2)transform.position + (direction * 2);
    }

    public void createNewSpell()
    {
        if (spellCount < limit +1)
        {
            GameObject spell = Instantiate(Resources.Load("Spells/" + getName(gameObject.name)) as GameObject);
            water4Script newSpellScript = spell.GetComponent<water4Script>();
            newSpellScript.spellCount = spellCount + 1;
            spell.transform.position = nextLoc;
            newSpellScript.left = left;
            newSpellScript.direction = direction;

            if (transform.eulerAngles.y == 180)
                spell.transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
      
    }
}
