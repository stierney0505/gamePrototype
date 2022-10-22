using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning1Script : MonoBehaviour, spell
{
    float damage;
    char type;
    

    private void Start()
    {
        type = 'L';
        damage = 10;
        GameObject player = GameObject.Find("WWPlayerCharacter");
        Vector2 startLoc = player.transform.position;
        rotate(startLoc, transform.position);
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
    public void createOnHiteffect() { }

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
    
    public void enableCollider() { EdgeCollider2D col = GetComponent<EdgeCollider2D>(); col.enabled = true; }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
}