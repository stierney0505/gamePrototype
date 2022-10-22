using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class golemDummyScript : MonoBehaviour, unitInterface
{
    [SerializeField] public float health;
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
            spellComponent.end();
            takeDamage(damage);
            GameObject hitEffect = Instantiate(Resources.Load(spellTypeHelper.getOnHitEffect(spellComponent.getType()))) as GameObject;
            hitEffect.transform.position = transform.position;
        }
    }
}
