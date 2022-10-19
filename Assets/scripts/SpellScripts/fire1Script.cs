using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEditor.UIElements;
using UnityEngine;

public class fire1Script : spell
{
    Vector2 endLoc;
    private Animator animator;
    int loops = 0;

    private void Start()
    {
        Vector2 start;
        endLoc = transform.position;
        GameObject player = GameObject.Find("PlayerCharacter");
        start = player.transform.position;
        animator = GetComponent<Animator>();
        transform.position = start;
        rotate(start, endLoc);
    }

    private void Update() //make fireball keep traveling in the direction
    {
        float step = 7.5f * Time.deltaTime;
        if (transform.position.Equals(endLoc)) { animator.SetTrigger("fade"); }
        transform.position = Vector2.MoveTowards(transform.position, endLoc, step);
        
    }
    public override Vector3 getVector()
    {

        Vector3 postion = Input.mousePosition;
        return postion;

    }

    public override void end()
    {
        Destroy(gameObject);
    }

    public void loop()
    {
        if (loops < 7) { loops++; }
        else { animator.SetTrigger("fade"); }
    }

    public void rotate(Vector2 start, Vector2 end)
    {
        float startX = start.x;
        float startY = start.y;
        float endX = end.x;
        float endY = end.y;
        float slope = (endY - startY) / (endX - startX);
        float rotation = Mathf.Rad2Deg*Mathf.Atan(slope);
        transform.Rotate(0, 0, rotation);

        if(endX < startX) { transform.localScale = new Vector2(transform.localScale.x * -1.0f, transform.localScale.y); }
        
    }
}


