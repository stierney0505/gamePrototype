using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class construct : MonoBehaviour
{   
    float damage, health;
    char type;
    Animator animator;
    public void setDamageType(float dmg, char typ) { damage = dmg; type = typ; health = 1.75f * dmg; }


    private void OnCollisionEnter2D(Collision2D col)
    {   string tag = col.collider.tag;
        Rigidbody2D rigidbody = col.rigidbody;
        float velocity = rigidbody.velocity.sqrMagnitude; //Takes the sqrMagnitude of the velocity vector to get the velocity, sqrMagnitude because its more efficient Then a sqrRoot operations
        if (tag == "Enemy" && col.gameObject.TryGetComponent<Unit>(out Unit enemyComponent))
        {
            if (velocity >= 4) //if the velocity is not greater than 5 then 
            {
                rigidbody.velocity = Vector2.zero;
                switch (velocity)
                {
                    case <= 9:
                        enemyComponent.takeDamage(damage);
                        animator.speed = 1;
                        break;
                    case <= 25:
                        enemyComponent.takeDamage(damage * 1.75f);
                        animator.speed = 1;
                        break;
                    default:
                        enemyComponent.takeDamage(damage * 2.5f);
                        animator.speed = 1;  
                        break;
                }
            }
        }
        else if (tag == "Player" && col.gameObject.TryGetComponent<PlayerScript>(out PlayerScript playerComponent))
        {
            if (velocity >= 25) //if the velocity is not greater than 5 then 
            {
                rigidbody.velocity = Vector2.zero;
                playerComponent.takeDamage(damage);
                animator.speed = 1;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    void removeConstruct() //method that just destroys itself
    {   
        Destroy(gameObject);
    }
}
