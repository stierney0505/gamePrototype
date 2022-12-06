using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onPosScript : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack, rotation; //to decide the damage number
    [SerializeField] char type; //to decide the spell type
    internal Collider2D col;
    


    private void Start()
    {
        col = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (rotation != 0) { transform.Rotate(0, 0, rotation); }
        else if (transform.position.x < player.transform.position.x)
            transform.eulerAngles = new Vector2(0, 180);
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

    public void enableCollider() {  
        col.enabled = true; }
    public void disableCollider() { col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
