using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class fire1Script : MonoBehaviour, spell //A script that works for most linear traveling spells
{
    Vector2 endLoc;
    Vector2 startLoc;
    private Animator animator;
    int loops = 0;
    [SerializeField] float damage;
    [SerializeField] char type;
    [SerializeField] float impactSpeed;
    float step;

    private void Start()
    {
        
        endLoc = transform.position;
        GameObject player = GameObject.Find("WWPlayerCharacter");
        startLoc = player.transform.position;
        animator = GetComponent<Animator>();
        transform.position = startLoc;
        rotateExtend(startLoc, endLoc, true);

    }

    private void Update() //make fireball keep traveling in the direction
    {
        step = 12.5f * Time.deltaTime;
        if (transform.position.Equals(endLoc)) { rotateExtend(startLoc, endLoc, false); }
        if (loops != -1) { transform.position = Vector2.MoveTowards(transform.position, endLoc, step); }
        else if(loops == -1) { transform.position = Vector2.MoveTowards(transform.position, endLoc, (step / 4.0f)); }
        
    }
    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;
    }

    public void end() { step = impactSpeed; animator.SetTrigger("fade"); }

    public void remove() { Destroy(gameObject); }

    public void createOnHiteffect() { }
    public void loop()
    {
        if (loops < 3) { loops++; }
        else { end(); }
    }

    public void rotateExtend(Vector2 start, Vector2 end, bool rotate)
    {
        float startX = start.x;
        float startY = start.y;
        float endX = end.x;
        float endY = end.y;
        
        if (rotate)
        {
            float slope = (endY - startY) / (endX - startX);
            float rotation = Mathf.Rad2Deg * Mathf.Atan(slope);
            transform.Rotate(0, 0, rotation);

            if (endX < startX) { transform.localScale = new Vector2(transform.localScale.x * -1.0f, transform.localScale.y); }
        }
        else 
        {
            float rise = endY - startY;
            float run = endX - startX;
            endLoc = new Vector2(endLoc.x + (run * 3.0f), startLoc.y + (rise*3.0f));
        }
    }
    public float getDamage() { return damage; }
    public char getType() { return type; }
}


