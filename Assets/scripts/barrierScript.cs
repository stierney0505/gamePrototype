using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrierScript : MonoBehaviour //This script just keeps the barrier objects on the player, the code for managing barrier is done through the PlayerScript for now
{
    SpriteRenderer sprite;
    private int parryState; //This int will mark the parry state, 2 for perfect parry, 1 for parry, 0 or less for blocking

    public int getParryState() { return parryState; } 
    
    private float blockHealth, maxHealth; //Blockhealth will be used to measure how much hitpoints is left in the barrier, and maxhealth is used to create a ratio to determine 
    //how opaque the barrier should be
    public void setHealth(float damage) { //A set health method, removes health equal to damage and sets the color equal to a new more opaque color
        blockHealth = blockHealth - damage; 
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, blockHealth / maxHealth); 
        if(blockHealth < 10 ) //If the health is less than 10 destorys the barrier 
            Destroy(gameObject);
    }
    public float getHealth() { return blockHealth; }
    void updateBlock() { setHealth(blockHealth / 1.75f); } //A method called in the animation that reduces the block health of the barrier
    void updateParry() { parryState--;} //A method called in the animation that reduces the parry state
    public void setPos(Transform pos) //this method sets the position of the barrier to the player and sets the block health
    {   
        transform.position = pos.position + new Vector3(0f, .25f, 0); 
        blockHealth = 100;
        maxHealth = 100;
        parryState = 2;
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}
