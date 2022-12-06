using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public class dark4Script : MonoBehaviour, spell //This spell will first start with one spell at the cursor location then attack X more times
{                                               //which distanceBetween +or- distanceBetween/2 within the angle of AttackAngle
    [SerializeField] float damage, knockBack, distanceBetween, attackAngle; 
    [SerializeField] char type;
    internal int spellCount;
    public int limit;
    internal Vector2 startLoc, direction;
    internal Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        if (spellCount == 0)
        {
            spellCount = 1;
            Vector2 endLoc = transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            startLoc= player.transform.GetChild(0).position;
            direction = (endLoc - startLoc).normalized;
            startLoc = transform.position;
        }
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
        col.enabled = true;
    }
    public void disableCollider() { col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public Vector2 getNextLoc()
    {
        float circleDiameter = 2 * ((Mathf.Tan(attackAngle/2)) * distanceBetween);
        Vector2 random = Random.insideUnitCircle;
        Vector2 random2 = new Vector2(random.x * circleDiameter/(spellCount), random.y);
        Vector2 nextLoc = startLoc + (direction*distanceBetween) + random2;
        
        return nextLoc;
    }

    public string getName(string objName)
    {
        int indexNum = objName.IndexOfAny("123456".ToCharArray());
        return objName.Substring(0, indexNum + 1);
    }

    public void createNewSpell()
    {
        if (spellCount < limit - 1) 
        {
            GameObject spell = Instantiate(Resources.Load("Spells/" + getName(gameObject.name)) as GameObject);
            dark4Script newSpellScript = spell.GetComponent<dark4Script>();
            newSpellScript.spellCount = 1 + spellCount;
            newSpellScript.startLoc = startLoc;
            newSpellScript.direction= direction;
            newSpellScript.distanceBetween = distanceBetween + (distanceBetween / spellCount);
            newSpellScript.attackAngle = attackAngle;
            spell.transform.position = getNextLoc();
        }
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
