using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ranged1Projectile : MonoBehaviour, spell
{
    [SerializeField] float damage, knockBack; //to decide the damage number
    dLRSNode.types type = dLRSNode.types.DARK;
    public float speed, aSpeed;
    GameObject targetEnemy;
    Animator animator;
    int currentWayPoint = 0, loops = 0;
    float time;
    bool ending = false;


    private void Start()
    {
        animator = GetComponent<Animator>();
        targetEnemy = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (targetEnemy != null)
            end(false);
        else if (!ending)
            travel();
    }
    private void travel()
    {

        Vector2 dir = targetEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * aSpeed);
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    public void remove() { Destroy(gameObject); }
    public float getDamage() { return damage; }
    public dLRSNode.types getType() { return type; }
    
    public void findPlayer()
    {

    }
    public float getKnockBack() { return knockBack; }

    public void end(bool environment)
    {
        ending = true;
        animator.SetTrigger("fade");
        disableHitBox();
    }

    public void disableHitBox() { Collider2D hitBox = GetComponent<Collider2D>(); hitBox.enabled= false; }

    void spell.addEnemy(Transform enemyTransform)
    {
        throw new System.NotImplementedException();
    }
}
