using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dark3Script : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number || For this spell the damage and knockback should be 1/4 of the total damage and Knockback
    [SerializeField] char type; //because the spell will hit twice, once with the black triangle and a second time with the hand
    internal CapsuleCollider2D capCol;
    internal PolygonCollider2D polyCol;
    private bool phase2 = false;


    private void Start()
    {
        capCol= GetComponent<CapsuleCollider2D>();
        polyCol= GetComponent<PolygonCollider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (transform.position.x < player.transform.position.x)
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

    public void enableCollider()
    {
        if(!phase2)
            polyCol.enabled = true;
        else
        {
            damage *= 3; //These just make the second part of the damage deal 3/4 of the total damage
            knockBack *= 3;
            capCol.enabled = true;
        }
    }
    public void disableCollider()
    {
        if (!phase2)
        {
            polyCol.enabled = false;
            phase2 = true;
        }
        else
        {
            capCol.enabled = false;
        }
    }
    public float getKnockBack() { return knockBack; }
}
