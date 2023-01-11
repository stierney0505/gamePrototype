using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class fire1Script : MonoBehaviour, spell //A script that works for most linear traveling spells
{
    Vector2 endLoc, startLoc, direction;
    private Animator animator;
    int loops = 0;
    [SerializeField] float damage, knockBack, speed;
    [SerializeField] dLRSNode.types type;

    private void Start()
    {
        
        endLoc = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        startLoc = player.transform.GetChild(0).position;
        animator = GetComponent<Animator>();
        transform.position = startLoc;
        direction = (endLoc - startLoc).normalized; //Vector pointing from startLoc to endLoc
        rotate();
    }

    private void Update() //make fireball keep traveling in the direction
    {

        if (transform.position.Equals(endLoc)) { extend(); }
        if (loops != -1) { transform.Translate(direction * Time.deltaTime * speed, Space.World); }


    }
    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;
    }

    public float getAngle(Vector2 start, Vector2 end)
    {
        return Mathf.Atan2(Vector3.Dot(Vector3.Cross(start, end), Vector3.back), Vector3.Dot(start, end)) * Mathf.Rad2Deg;
    }

    public void end(bool environment)
    {
        speed = 0; animator.SetTrigger("fade");
    }

    public void remove() { Destroy(gameObject); }
    public void loop()
    {
        if (loops < 3) { loops++; }
        else { end(false); }
    }

    public void rotate()
    {
        float startX = startLoc.x;
        float startY = startLoc.y;
        float endX = endLoc.x;
        float endY = endLoc.y;


        float slope = (endY - startY) / (endX - startX);
        float rotation = Mathf.Rad2Deg * Mathf.Atan(slope);
        transform.Rotate(0, 0, rotation);

        if (endX < startX) { transform.localScale = new Vector2(transform.localScale.x * -1.0f, transform.localScale.y); }
    }

    public void extend()
    {


        float startX = startLoc.x;
        float startY = startLoc.y;
        float endX = endLoc.x;
        float endY = endLoc.y;

        float rise = endY - startY;
        float run = endX - startX;
        endLoc = new Vector2(endLoc.x + (run * 3.0f), startLoc.y + (rise * 3.0f));

    }
    public float getDamage() { return damage; }
     public dLRSNode.types getType() { return type; }

    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}


