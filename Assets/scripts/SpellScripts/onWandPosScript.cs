using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onWandPosScript : MonoBehaviour, spell
{
    public float damage, knockBack;
    public char type;
    

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerSource = player.transform.GetChild(0).position;
        rotate(playerSource, transform.position);
        transform.position = playerSource;
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
    public void rotate(Vector2 start, Vector2 end)
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
    
    public void enableCollider() {Collider2D col = GetComponent<Collider2D>(); col.enabled = true; }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }
}
