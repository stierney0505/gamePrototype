using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dark2Script : MonoBehaviour, spell
{
    public float damage, knockBack;
    public char type;
    public int spellCount, limit;
    internal Vector2 playerSource, startLoc;


    private void Start()
    {
        if (spellCount == 1)
        {   
            startLoc = transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerSource = player.transform.GetChild(0).position;
        }
        rotate(playerSource, startLoc);
        transform.position = playerSource;
        if(spellCount%2 == 0) 
            transform.localScale = new Vector2(transform.localScale.x,-transform.localScale.y);
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
    public void rotate(Vector2 start, Vector2 end) //Todo create variance in the rotation probably -30/30
    {
        if (start.x > end.x) { transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); }
        float startX = start.x;
        float startY = start.y;
        float endX = end.x;
        float endY = end.y;
        float slope = (endY - startY) / (endX - startX);
        float rotation = Mathf.Rad2Deg * Mathf.Atan(slope);
        transform.Rotate(0, 0, rotation);
    }

    public void enableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = true; }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

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
            dark2Script newSpellScript = spell.GetComponent<dark2Script>();
            newSpellScript.spellCount = spellCount + 1;
            newSpellScript.playerSource= playerSource;
            newSpellScript.startLoc= startLoc;
            spell.transform.position = transform.position;
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
