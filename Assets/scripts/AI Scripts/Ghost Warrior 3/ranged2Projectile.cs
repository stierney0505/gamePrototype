using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ranged2Projectile : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] dLRSNode.types type; //to decide the spell type
    int spellCount = 0;
    public int limit;
    Vector2 nextLoc, direction;


    private void Start()
    {
        if (spellCount == 0)
        {
            Vector2 startLoc, endLoc;
            spellCount = 1;
            startLoc = transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            endLoc = player.transform.GetChild(0).position;

            direction = (endLoc - startLoc).normalized; //Vector pointing from startLoc to endLoc
            getNextPoint();
        }
        else
            getNextPoint();
    }
    public void remove() { Destroy(gameObject); }
    public void end(bool environment) {
        if (environment)
        {   SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
            disableCollider();
            renderer.enabled = false;

        }
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

    public void getNextPoint()
    {
        nextLoc = (Vector2)transform.position + (direction * 2);
    }

    public void createNewSpell() //range currently for 8 of these is 14 so the range would be 1.75 * (limit + 1)
    {
        if (spellCount < limit + 1)
        {
            GameObject spell = Instantiate(Resources.Load("Enemies/EnemyProjectiles/GhostWarrior3Projectile2") as GameObject);
            ranged2Projectile newSpellScript = spell.GetComponent<ranged2Projectile>();
            newSpellScript.spellCount = spellCount + 1;
            spell.transform.position = nextLoc;
            newSpellScript.direction = direction;

            if (transform.eulerAngles.y == 180)
                spell.transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void addEnemy(Transform enemyTransform)
    {

    }
}
