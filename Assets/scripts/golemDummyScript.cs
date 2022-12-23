using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golemDummyScript : MonoBehaviour, unitInterface
{
    [SerializeField] public float health;
    public GameObject player;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        health = 100.0f;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = getAngle();
        if(Mathf.Abs(angle) < 20)
            transform.Rotate(0, 0, angle);
        transform.position += transform.right * .4f * Time.deltaTime;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) { health = 0; die(); }
    }

    public void die() { animator.SetTrigger("Death"); }
    
    public void remove() { Destroy(gameObject); }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<spell>(out spell spellComponent))
        {
            float damage = spellComponent.getDamage();
            char type = spellComponent.getType();
            spellComponent.end(false);
            takeDamage(damage);
            GameObject hitEffect = Instantiate(Resources.Load(spellTypeHelper.getOnHitEffect(spellComponent.getType()))) as GameObject;
            hitEffect.transform.position = transform.position;
        }
    }

    float getAngle()
    {
        float angle;
        Vector2 forward = transform.right;
        Vector2 playerDirection = player.transform.position - transform.position;
        angle = Vector2.Angle(forward, playerDirection);
        float crossProd = forward.x * playerDirection.y - forward.y * playerDirection.x;
        int rotation = 1;
        if(crossProd < 0) { rotation = -1; }
        
        return angle * rotation;
    }
    } 

    
