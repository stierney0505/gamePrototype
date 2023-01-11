using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire5Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    public int spellCount, limit;
    internal Vector2 startLoc, endLoc, direction;
    internal bool left;


    private void Start()
    {
        if (spellCount == 1)
        {
            endLoc = transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            startLoc = player.transform.GetChild(0).position;
            direction = (endLoc - startLoc).normalized;
            if (transform.position.x < player.transform.position.x) { left = true; }
            transform.position = startLoc;
            transform.position = getNextLoc();
        }
        if(spellCount%2 != 0)
            transform.localScale = new Vector2(transform.localScale.x, -transform.localScale.y);
        if(left)
            transform.eulerAngles = new Vector2(0, 180);
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

    public Vector2 getNextLoc()
    {
        Vector2 nextLoc = (Vector2)transform.position + (direction*2.4f);
        return nextLoc;
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
            fire5Script newSpellScript = spell.GetComponent<fire5Script>();
            newSpellScript.spellCount = 1 + spellCount;
            newSpellScript.direction = direction;
            newSpellScript.left = left;
            spell.transform.position = getNextLoc();
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
