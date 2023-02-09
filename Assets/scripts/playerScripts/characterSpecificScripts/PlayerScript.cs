using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    spellSelector selector;
    private dLRSNode.types[] runes;
    public float speed = 5.0f, health = 100.0f, mana = 50.0f;
    protected int runeCount, //runeCount is the count of the runes used in a spell
        comboCount, //ComboCount will be incremented up to three based upon how many times the player clicks during the combo window
        comboCheck = 0, //ComboCheck increments at the end of each comboAttack as a way to check when the combo is done
        manaDrain = 15;
    private Animator animator;
    private bool isStruck = false, 
        chargeActive = false, //this bool keeps track of if the character is charging a rune into the rune buffer
        altFire = false,  //altFire bool is true when the player uses the empowered spell, and it lets launchspell know to empty the rune buffer
        barActive = false, //barActive bool is for checking if the character is using a barrier
        comboActive = false, //comboActice bool is for checking if the character is performing a combo
        movementDisabled = false, dead = false, barrierActive = false, barrierDisabled = false,
        up = false, down = false, left = false, right = false; //I used 4 bools to determine what directions the player is pressing
    Rigidbody2D body;
    GameObject barrier;
    SpriteRenderer sprite;
    runeSelector runeSelector;
    public dLRS list; //List of the runes the player has
    dLRSNode.types nextSpellType; //The next spell that will be alt-fired, determined by either the last rune in the buffer or a combination of them
    Vector2 mousePos; //this vector2 keeps track of the mousePos that the player clicked on to
    string spellName;

    void Start()
    {   //get needed components
        
        animator = GetComponent<Animator>();
        healthBar.setHealthBarValue(health);
        manaBar.setManaBarValue(mana);
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        runeSelector = GetComponent<runeSelector>();
        selector = GetComponent<spellSelector>(); 

        runes = new dLRSNode.types[5];
        runes[0] = dLRSNode.types.EMPTY;
        runeCount = 0;

        dLRSNode.types[] elements = new dLRSNode.types[6];
        switch (gameObject.name[0]) //Each character gets only 2 elements to charge/use basic attacks with, so the first letter of the character is either W, R, or B 
        {                           //For the white, red, and blue witch respectively and each will get two elements based upon that.
            case 'N':
                elements[0] = dLRSNode.types.FIRE;
                elements[1] = dLRSNode.types.DARK;
                elements[2] = dLRSNode.types.EARTH;
                elements[3] = dLRSNode.types.WATER;
                elements[4] = dLRSNode.types.AIR;
                elements[5] = dLRSNode.types.LIGHTNING;
                runeSelector.setIcon(dLRSNode.types.FIRE);
                break;
        }

        list = dLRS.createList(elements);

        animator.SetLayerWeight(getLayer(), 1);

    }

    
    void Update()
    {

        if (!(up || down || left || right)) //This stops the character's move animation the moment they stop
            animator.SetBool("running", false);

        if (!barrierDisabled && Input.GetKeyDown(KeyCode.LeftShift)) { createBlock(); } //TODO ADD BOOL FOR IF HIT/KNOCKBACK AND MAKE ALL ACTIONS NOT POSSIBLE DURING
        if (barrierActive && !Input.GetKey(KeyCode.LeftShift)) { destroyBlock(); }

        
            if (Input.GetMouseButtonDown(1) && runeCount != 0 && !comboActive) //This checks if the player right clicked and has enough runes to use a specialattack (runes>0), and that a melee combo isn't being performed
            {
                altFire = true; //Adjust the altfire bool because a special attack is will be performed
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(nextSpellType, runeCount);
                animator.SetLayerWeight(getLayer(dLRSNode.toStr(nextSpellType)), 1);
                animator.SetTrigger("specialAttack");
                chargeActive = false;
                if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); } //rotates the player towards the direction they clicked
                else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (comboActive == false) { comboCount = 1; comboActive = true; } //These line checks if the character is in the middle of a combo, and if not sets the combo bool to true
                else if (comboActive == true && comboCount < 3) { comboCount++; }

                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                spellName = selector.spellSelect(list.getData());
                animator.SetBool("attack" + comboCount, true);
                chargeActive = false;
                if (mousePos.x < transform.position.x && transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); } //rotates the player towards the direction they clicked
                else if (mousePos.x > transform.position.x && transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            }

        if (Input.GetKeyDown(KeyCode.E) && !movementDisabled && manaBar.getManaBarValue() > 0) {  //The E button currently turns the character into a charge state where they 
               animator.SetTrigger("charge");   chargeActive = true;}     //triggers the charge animation

        if (Input.GetKey(KeyCode.W)) //these get the directions the character is moving as bools, this is so that if 
            up = true;               //Up and down is pressed the character doesnt move
        if (Input.GetKey(KeyCode.S))
            down = true;
        if (Input.GetKey(KeyCode.A))
            left = true;
        if (Input.GetKey(KeyCode.D))
            right = true;
        Move(up, down, left, right);

        up = false; down = false; left = false; right = false;

        if (chargeActive == true && manaBar.getManaBarValue() > 0)
        {
            runeBufferProgress.incrementProgress(4 * manaDrain *Time.deltaTime); //Increments the progress bar by a proportion of the mana used over time
            manaBar.incrementPlayerMana(-manaDrain * Time.deltaTime, true); //Mana is currently removed at a set amount, will be changed once the game is more realized 
        }
        else if (manaBar.getManaBarValue() <= 0) { chargeActive = false; animator.SetTrigger("idle"); }

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
            chargeActive = false;
            Vector2 movement = new Vector3(0, 1.0f, 1.0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveDown)
        {
            animator.SetBool("running", true);
            chargeActive = false;
            Vector2 movement = new Vector3(0, -1.0f, -1.0f);
            transform.Translate(movement * speed * Time.deltaTime);
        }
        if (moveLeft)
        {
            if (transform.eulerAngles.y == 0) { transform.eulerAngles = new Vector2(0, 180); }
            animator.SetBool("running", true);
            chargeActive = false;
            transform.Translate(-transform.right.normalized * speed * Time.deltaTime);
        }
        if (moveRight)
        {
            if (transform.eulerAngles.y == 180) { transform.eulerAngles = new Vector2(0, 0); }
            animator.SetBool("running", true);
            chargeActive = false;
            transform.Translate(transform.right.normalized * speed * Time.deltaTime);
        }
    }

    public void takeDamage(float damage) //Damage method, triggers hit animation and modifies the player health bar
    {
        if (dead)
            return;
        health -= damage;
        if (health < 0) { health = 0; die(); }
        else
            animator.SetTrigger("hit");
        healthBar.incrementPlayerHealth(-damage);
        healthBar.healthBarColor();
    }

    public void die() { 
        animator.SetLayerWeight(getLayer(), 1);
        animator.SetTrigger("dead"); 
        dead = true; 
        Collider2D col = GetComponent<Collider2D>(); 
        col.enabled = false;
        body.velocity = Vector2.zero;
    } 
    public void stopMovement() { movementDisabled = true; animator.SetBool("running", false); } //Stops the player from moving 
    public void allowMovement() { movementDisabled = false; } //Enables moving
    
    public void turnRed() { sprite = GetComponent<SpriteRenderer>(); sprite.color = Color.red; stopMovement(); barrierDisabled = true; }
    public void turnWhite() {  sprite = GetComponent<SpriteRenderer>(); sprite.color = Color.white; allowMovement(); barrierDisabled = false; }
    
    private void OnTriggerEnter2D(Collider2D col) //This currently caculates the damage the player takes based on the barrier and collision
    {   
        Unit enemyComponent = null; //This component is for an enemy melee attack
        spell enemyProjectile = null; //This component is for an enemy ranged attack
        float damage = 0, tempDmg = 0, knockBack = 0; //floats for the inital total dmg, the actual dmg the player takes, and knockback
        dLRSNode.types type = dLRSNode.types.EMPTY; //The type of the attack
        
        if (col.tag == "Attack") //If its tag is "attack" its a melee attack
        {
            enemyComponent = col.GetComponentInParent<Unit>(); //Set the intial values above to the value of the enemy
            damage = enemyComponent.getDamage();
            type = enemyComponent.getType();
            knockBack = enemyComponent.getKnockBack();
        }
        else if (col.tag == "EnemyProjectile") //If its tag is EnemyProjectile is a ranged attack
        {
            enemyProjectile = col.GetComponent<spell>(); //Set the intial values above to the value of the enemy
            damage = enemyProjectile.getDamage();
            type = enemyProjectile.getType();
            knockBack = enemyProjectile.getKnockBack();
            enemyProjectile.end(false);
        }

        if (enemyComponent != null || enemyProjectile != null) //This checks if 
        {   
            if (barrier != null) //Checks if the barrier is active
            {
                barrierScript barrierScr = barrier.GetComponent<barrierScript>();//Gets the barrierscript
                switch (barrierScr.getParryState()) //Switch statement for the parry state, 2 for perfect parry, 1 for parry, 0 for block
                {   //TODO add knock back and some sort of parry reflect damage
                    case 2 : //If its a perfect parry damage is ignored and runebuffer progresses
                        runeBufferProgress.incrementProgress(spellTypeHelper.damageModifier(type, list.getData(), (damage*1.5f)));
                        return;
                    case 1 : //If its a parry the damage is halfed
                        tempDmg = (damage / 2) - barrierScr.getHealth(); //Modifies the actual damage and barrier health
                        barrierScr.setHealth(barrierScr.getHealth() - (damage / 2));
                        if (tempDmg <= 0) //if the barrier withstand the hit then it progress the rune buffer
                        {
                            runeBufferProgress.incrementProgress(spellTypeHelper.damageModifier(type, list.getData(), damage));
                            return;
                        }
                        destroyBlock();//If the code reaches here the block failed, so it is destroyed
                        break;

                    default: 
                        tempDmg = damage - barrierScr.getHealth(); //Set tempdamage to 
                        barrierScr.setHealth(barrierScr.getHealth() - (damage));
                        if (tempDmg <= 0)
                        {
                            runeBufferProgress.incrementProgress(spellTypeHelper.damageModifier(type, list.getData(), damage * 0.5f));
                            return;
                        }
                        destroyBlock();
                        break;
                }
            }
            
   
            if(tempDmg > 0)  //If tempDmg is greater than 0 then a barrier modified the damage
                takeDamage(spellTypeHelper.damageModifier(type, list.getData(), tempDmg)); //Calls static spellTypeHelper method to modify the damage based on what rune is selected in the rune selector
            else //If tempDmg is less than 0 and the code reaches here, then tempDmg was never changed and the player takes full damage
                takeDamage(spellTypeHelper.damageModifier(type, list.getData(), damage));
            Vector2 forceDirection = transform.position - col.transform.position; //create knockback vector
            body.AddForce(forceDirection.normalized * knockBack, ForceMode2D.Impulse); //take knockback
        }
    }
    private void createBlock()
    {
        chargeActive = false;
        animator.SetTrigger("idle");
        animator.SetBool("running", false);
        barrierActive = true;
        dLRSNode.types type = list.getData();
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
        dLRSNode.types type = list.getData();

        switch (type)
        {
            case dLRSNode.types.FIRE:
                return "fCharge";
            case dLRSNode.types.EARTH:
                return "eCharge";
            case dLRSNode.types.WATER:
                return "wCharge";
            case dLRSNode.types.AIR:
                return "aCharge";
            case dLRSNode.types.LIGHTNING:
                return "lCharge";
            case dLRSNode.types.DARK:
                return "dCharge";
            default:
                return null;
        }
    }
    public void launchSpell()
    {
        if (comboCheck == 2)//This if statement checks if the combo is on the third(Final) attack of the combo, comboCheck gets incremented at the end of an attack so it needs to be 2 to check for the third attack during the third attack
            spellName = selector.spellSelect(list.getData(), 1);//Upgrades the attack to level two through the spellSelect method

        GameObject spell = Instantiate(Resources.Load("Spells/" + spellName)) as GameObject;
        if (Input.GetMouseButton(0) && !altFire) //This checks if the mouse is being held during the launch of a combo, and if so
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //readjusts the mousePos for the lauching of the spell
        spell.transform.position = mousePos;
        spell.SetActive(true);
        if (altFire) //altFire bool tracks if the rune buffer was used to empower the spell, so if it is true then it removes the runes in the buffer
        {
            for (int i = 0; i < runes.Length; i++) { runes[i] = dLRSNode.types.EMPTY; };
            runeSelector.disableRuneIcons();
            selector.disableChildren();
            altFire = false;
            runeBufferProgress.resetBarProgress();
        }
    }

    public int getLayer() //This method checks if the current rune equipped is the same type as the animator layer through the toStr method
    {                     //this method also makes each layer than isn't the layer of the selected equal to 0
        int returnInt = 0;
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (list.toStr() == animator.GetLayerName(i))
                returnInt = i;
            else
                animator.SetLayerWeight(i, 0); //TODO once the amount of runes are selectable, optimize the code so that it only turns off the x-1 selected runes

        }
        return returnInt;
    }
    public int getLayer(string goalLayer) //This is an overload of the getLayer() method, this has a parameter as a goal layer to set as the top layer
    {
        int returnInt = 0;
        for (int i = 0; i < animator.layerCount; i++)
        {
            if (goalLayer == animator.GetLayerName(i)) //checks if the current layer is the goal layer
                returnInt = i;
            else
                animator.SetLayerWeight(i, 0); //This sets each non-goal layer's weight to 0 (invisible)

        }
        return returnInt;
    }
    public void resetCombo()
    { //This method resets the comboActive bool and comboCount int, called through the animator at the end of each combo animation
        comboCheck++;
        if (Input.GetMouseButton(0) && comboCount < 3) //This if statement checks if the left click is being held down, if so
        {
            comboCount++; //it increments the comboCount
            animator.SetBool("attack" + comboCount, true); //Advances the animation to the next stage of the combo attack
            return; //ends the method
        }
        else if (comboCount > comboCheck)//Since comboCount goes up to 3, if comboCheck is ever equal to comboCount then the comboShould end
            return;

        comboActive = false; comboCount = 0; comboCheck = 0;
        animator.SetBool("attack1", false); animator.SetBool("attack2", false); animator.SetBool("attack3", false);
    }
    public void resetAnimatorLayer() { animator.SetLayerWeight(getLayer(), 1); } //This method switches the animator layer back to the element selected in the rune buffer, called through the animator at the end of a special attack or every hurt/death animation
    public void onHitBoolReset()
    {
        altFire = false; comboActive = false; comboCount = 0; comboCheck = 0; chargeActive = false;
        animator.SetBool("attack1", false); animator.SetBool("attack2", false); animator.SetBool("attack3", false);
        chargeActive = false; barActive = false; //This method just resets all the values/bools to their base state, intended for use when the player gets struck, called through the animator
    }

    public bool isDead() { return dead; }
    public void replace() //This method is called through the animator and replace the corpse of the player with an uninteractable object
    {
        string prefix = name.Substring(0, 2);
        GameObject deathObject = Instantiate(Resources.Load(prefix + "dead")) as GameObject;
        deathObject.transform.rotation = transform.rotation;
        Destroy(gameObject);
    }

    public void endCharge() { chargeActive = false; runeBufferProgress.incrementProgress(10); }

    public dLRS getList() { return list; }
    public int getRuneCount() { return runeCount; }
    public dLRSNode.types [] getRunes() { return runes; }
    public dLRSNode.types getNextSpellType() { return nextSpellType; }
    public void setList(dLRS newList) { list = newList; }
    public void setRuneCount(int num) { runeCount = num; }
    public void setRunes(dLRSNode.types[] newRunes) { runes = newRunes; }
    public void setNextSpellType(dLRSNode.types newType) { nextSpellType = newType; }

}
