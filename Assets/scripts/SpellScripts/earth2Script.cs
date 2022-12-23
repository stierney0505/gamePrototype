using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earth2Script : MonoBehaviour, spell
{
    float time = 0;
    [SerializeField] float damage, knockBack;
    [SerializeField] char type;

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.speed = 1.25f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (transform.position.x < player.transform.position.x)
            transform.eulerAngles = new Vector2(0, 180);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 10)
            remove();
    }

    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;
    }

    public void remove() { Destroy(gameObject); }
    public void end(bool environment)
    {
        if (environment)
            remove();
    }

    public float getDamage() { return damage; }
    public char getType() { return type; }
    public float getKnockBack() { return knockBack; }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
