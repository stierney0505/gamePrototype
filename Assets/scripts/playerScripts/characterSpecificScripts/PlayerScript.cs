using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 5.0f;
    public float health = 100.0f;
    private Animator animator;
    runeSelector runeSelector;
    bool movementDisabled = false, dead = false, barrierActive = false, barrierDisabled = false; 
    Rigidbody2D body;
    GameObject barrier;
    SpriteRenderer sprite;

    void Start()
    {   //get needed components
        runeSelector = GetComponent<runeSelector>();
        animator = GetComponent<Animator>();
        healthBar.setHealthBarValue(health);
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        bool up = false, down = false, left = false, right = false; //I used 4 bools to determine what directions the player is pressing

        if (Input.GetKeyDown(KeyCode.E) && !movementDisabled) //The E button currently turns the character into a charge state where they 
            Charge();                    //Load a rune into the rune buffer

        if (Input.GetKey(KeyCode.W)) //these get the directions the character is moving as bools, this is so that if 
            up = true;               //Up and down is pressed the character doesnt move
        if (Input.GetKey(KeyCode.S))
            down = true;
        if (Input.GetKey(KeyCode.A))
            left = true;
        if (Input.GetKey(KeyCode.D))
            right = true;
        Move(up, down, left, right);

        if (!(up || down || left || right)) //This stops the character's move animation the moment they stop
            animator.SetBool("running", false);

        if (!barrierDisabled && Input.GetKeyDown(KeyCode.LeftShift) && !runeSelector.isChargeActive()) { createBlock(); } //TODO ADD BOOL FOR IF HIT/KNOCKBACK AND MAKE ALL ACTIONS NOT POSSIBLE DURING
        if (barrierActive && !Input.GetKey(KeyCode.LeftShift)) { destroyBlock(); }
        //if (barrier == null) { movementDisabled = false; } 
    }

    public void Move(bool up, bool down, bool left, bool right) //Movement method, just takes the bools for movement directions
    {                                                           //and moves the character based upon that
        if(movementDisabled || dead)
            return;
        
        bool moveUp = (up && !down); //these bools determine if the character should move up, i.e. if up is pressed and not down etc.
        bool moveDown = (down && !up);
        bool moveLeft = (left && !right);
        bool moveRight = (right && !left);

        if (moveUp) //these if statements move the player based on the previous bools
        {
            animator.SetBool("running", true);
            Vector2 movement = new Vector3(0, 1.0f, 1.0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveDown)
        {
            animator.SetBool("running", true);
            Vector2 movement = new Vector3(0, -1.0f, -1.0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveLeft)
        {
            if (transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); }
            animator.SetBool("running", true);
            transform.Translate(-transform.right.normalized * speed * Time.deltaTime);
        }
        if (moveRight)
        {
            if (transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            animator.SetBool("running", true);
            transform.Translate(transform.right.normalized * speed * Time.deltaTime);
        }
    }

    public void Charge() { animator.SetTrigger(getChargeType()); runeSelector.createChargeCircle(); } //simple method to trigger charge animation 
    
    public void takeDamage(float damage) //Damage method, triggers hit animation and modifies the player health bar
    {
        spellSelector.staticDestoryCircle();
        if (dead)
            return;
         health -= damage;
         if (health < 0) { health = 0; die(); }
         else
            animator.SetTrigger("hit");
         healthBar.incrementPlayerHealth(-damage);
         healthBar.healthBarColor();
    }

    public void die() { animator.SetTrigger("dead"); dead = true; } 
    public void stopMovement() { movementDisabled = true; animator.SetBool("running", false); } //Stops the player from moving 
    public void allowMovement() { movementDisabled = false; } //Enables moving
    
    public void turnRed() { sprite = GetComponent<SpriteRenderer>(); sprite.color = Color.red; stopMovement(); barrierDisabled = true; }
    public void turnWhite() {  sprite = GetComponent<SpriteRenderer>(); sprite.color = Color.white; allowMovement(); barrierDisabled = false; }
    
    private void OnTriggerEnter2D(Collider2D col)
    {   
        Unit enemyComponent = null;
        spell enemyProjectile = null;
        float damage = 0, tempDmg = 0, knockBack = 0;
        char type = 'X';
        
        if (col.tag == "Attack")
        {
            enemyComponent = col.GetComponentInParent<Unit>();
            damage = enemyComponent.getDamage();
            type = enemyComponent.getType();
            knockBack = enemyComponent.getKnockBack();
        }
        else if (col.tag == "EnemyProjectile")
        {
            enemyProjectile = col.GetComponent<spell>();
            damage = enemyProjectile.getDamage();
            type = enemyProjectile.getType();
            knockBack = enemyProjectile.getKnockBack();
            enemyProjectile.end(false);
        }

        if (enemyComponent != null || enemyProjectile != null)
        {   
            if (barrier != null)
            {
                barrierScript barrierScr = barrier.GetComponent<barrierScript>();
                switch (barrierScr.getParryState())
                {   //TODO add knock back and some sort of parry reflect damage
                    case 2 :
                        runeSelector.AddRune(type);
                        runeSelector.AddRune(type);
                        runeSelector.AddRune(type);
                        return;
                    case 1 :
                        tempDmg = (damage / 2) - barrierScr.getHealth();
                        barrierScr.setHealth(barrierScr.getHealth() - (damage / 2));
                        if (tempDmg <= 0)
                        {
                            runeSelector.AddRune(type);
                            runeSelector.AddRune(type);
                            return;
                        }
                        damage = tempDmg;
                        destroyBlock();
                        break;

                    default:
                        tempDmg = damage - barrierScr.getHealth();
                        barrierScr.setHealth(barrierScr.getHealth() - (damage));
                        if (tempDmg <= 0)
                        {
                            runeSelector.AddRune(type);
                            return;
                        }
                        destroyBlock();
                        break;
                }
            }
            
            // char type = spellComponent.getType(); TODO add enemy type
            if(tempDmg > 0)  
                takeDamage(tempDmg); 
            else
                takeDamage(damage);
            Vector2 forceDirection = transform.position - col.transform.position;
            body.AddForce(forceDirection.normalized * knockBack, ForceMode2D.Impulse);
        }
    }
    private void createBlock()
    {
        animator.SetBool("running", false);
        barrierActive = true;
        char type = runeSelector.list.getData();
        runeSelector.setBarBool(true);
        barrier = Instantiate(Resources.Load("Barriers/" + type + "Barrier")) as GameObject;
        barrier.GetComponent<barrierScript>().setPos(transform);
        stopMovement();
    }
    private void destroyBlock()
    {
        barrierActive = false;
        if (barrier != null)
            Destroy(barrier.gameObject); 
        allowMovement();
        runeSelector.setBarBool(false);
    }

    public string getChargeType() //This is a helper method to determine which charge animation to play based on the rune
    {
        char type = runeSelector.list.getData();

        switch (type)
        {
            case 'F':
                return "fCharge";
            case 'E':
                return "eCharge";
            case 'W':
                return "wCharge";
            case 'A':
                return "aCharge";
            case 'L':
                return "lCharge";
            case 'D':
                return "dCharge";
            default:
                return null;
        }
    }

    public void stopAnimation() { 
        animator.speed = 0; }
    public bool isDead() { return dead; }
}
