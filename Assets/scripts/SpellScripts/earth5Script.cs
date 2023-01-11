using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth5Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    public int spellCount;
    Vector2 endLoc, startLoc;
    float rise, run;
    bool left;//if the spell is to the left of the player this is true


    private void Start()
    {
        if (spellCount == 1)
        {
            startLoc = transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (transform.position.x < player.transform.position.x)
            {   transform.eulerAngles = new Vector2(0, 180);
                endLoc = new Vector2(startLoc.x - 1f, startLoc.y);
                left = true;
            }
            else
                endLoc = new Vector2(startLoc.x + 1f, startLoc.y);
        }
        else
        {
            startLoc = transform.position;
            if (left)
            {
                transform.eulerAngles = new Vector2(0, 180);
                endLoc = new Vector2(startLoc.x - 1f, startLoc.y);
            }
            else
                endLoc = new Vector2(startLoc.x + 1f, startLoc.y);
        }
        
        extend();
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
     public dLRSNode.types getType() { return type; }

    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void createConstruct()//Creates an construct object based on the name of the object the script is attached to
    {
        string name = getName(gameObject.name);
        GameObject construct = Instantiate(Resources.Load("Constructs/" + name + "Construct")) as GameObject;
        if (transform.eulerAngles.y == 180)
            construct.transform.eulerAngles = new Vector2(0, 180);
        construct.GetComponent<construct>().setDamageType(damage, type);
        construct.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public string getName(string objName)
    {
        int indexNum = objName.IndexOfAny("123456".ToCharArray());
        return objName.Substring(0, indexNum + 1);
    }

    public void extend()
    {
        float startX = startLoc.x;
        float startY = startLoc.y;
        float endX = endLoc.x;
        float endY = endLoc.y;

        rise = endY - startY;
        run = endX - startX;
        endLoc = new Vector2(endLoc.x + (run * .75f), startLoc.y + (rise * .75f));
    }

    public void createNewSpell()
    {
        if (spellCount < 4)
        {
            GameObject spell = Instantiate(Resources.Load("Spells/" + getName(gameObject.name)) as GameObject);
            earth5Script newSpellScript = spell.GetComponent<earth5Script>();
            newSpellScript.spellCount += spellCount;
            spell.transform.position = endLoc;
            newSpellScript.left = left;

            if (transform.eulerAngles.y == 180)
                spell.transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
