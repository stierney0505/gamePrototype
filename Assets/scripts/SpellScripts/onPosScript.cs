using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onPosScript : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] char type; //to decide the spell type
    


    private void Start()
    {
        
        GameObject player = GameObject.Find("WWPlayerCharacter");
        if(transform.position.x < player.transform.position.x)
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
        Collider2D col = GetComponent<Collider2D>(); 
        col.enabled = true; }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }
}
