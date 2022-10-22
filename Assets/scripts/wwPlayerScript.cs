using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wwPlayerScript : MonoBehaviour //Mostly A copy paste of the player script with extra animations, now the main character 10/20/2022
{

    private float speed = 5.0f;
    public float health = 100.0f;
    private Animator animator;
    runeSelector runeSelector;
    bool movementDisabled = false;
   

    
    void Start()
    {   //get needed components
        runeSelector = GetComponent<runeSelector>();
        animator = GetComponent<Animator>();
        healthBar.setHealthBarValue(health);

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
            animator.SetTrigger("still");

    }

    public void Move(bool up, bool down, bool left, bool right) //Movement method, just takes the bools for movement directions
    {                                                           //and moves the character based upon that
        if(movementDisabled)
            return;
        bool moveUp = (up && !down); //these bools determine if the character should move up, i.e. if up is pressed and not down etc.
        bool moveDown = (down && !up);
        bool moveLeft = (left && !right);
        bool moveRight = (right && !left);

        if (moveUp) //these if statements move the player based on the previous bools
        {
            animator.SetTrigger("playerMove");
            Vector3 movement = new Vector3(0, 1.0f, 1.0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveDown)
        {
            animator.SetTrigger("playerMove");
            Vector3 movement = new Vector3(0, -1.0f, -1.0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveLeft)
        {
            if(transform.localScale.x > 0) { gameObject.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); }
            animator.SetTrigger("playerMove");
            Vector3 movement = new Vector2(-1.0f, 0);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveRight)
        {
            if (transform.localScale.x < 0) { gameObject.transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); }
            animator.SetTrigger("playerMove");
            Vector3 movement = new Vector2(1.0f, 0);
            transform.Translate(movement * speed * Time.deltaTime);
        }
    }

    public void Charge() { animator.SetTrigger(getChargeType()); runeSelector.locked = true; } //simple method to trigger charge animation 
    
    public void takeDamage(float damage) //Damage method, triggers hit animation and modifies the player health bar
    {
            animator.SetTrigger("hit");
            health -= damage;
            if (health < 0) { health = 0; die(); }
            healthBar.incrementPlayerHealth(-damage);
            healthBar.healthBarColor();
    }

    public void die() { animator.SetTrigger("dead"); } 
    public void stopMovement() { movementDisabled = true; } //Stops the player from moving 
    public void allowMovement() { movementDisabled = false; } //Enables moving

    public string getChargeType() //This is a helper method to determine which charge animation to play based on the rune
    {
        char type = runeSelector.list.getData();
        if (type == 'F') return "fCharge";
        else if (type == 'A') return "aCharge";
        else if (type == 'L') return "lCharge";
        else if (type == 'E') return "eCharge";
        else if (type == 'W') return "wCharge";
        return null;
    }
    
    public void turnRed() { SpriteRenderer sprite = GetComponent<SpriteRenderer>(); sprite.color = Color.red; }
    public void turnWhite() { SpriteRenderer sprite = GetComponent<SpriteRenderer>(); sprite.color = Color.white; }
}
