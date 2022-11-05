using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrierScript : MonoBehaviour //This script just keeps the barrier objects on the player, the code for managing barrier is done through the PlayerScript for now
{
    SpriteRenderer sprite;
    private int parryState; //This int will mark the parry state, 2 for perfect parry, 1 for parry, 0 or less for blocking

    public int getParryState() { return parryState; }
    
    private float blockHealth, maxHealth;
    public void setHealth(float damage) { 
        blockHealth = blockHealth - damage; 
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, blockHealth / maxHealth); 
        if(blockHealth < 10 )
            Destroy(gameObject);
    }
    public float getHealth() { return blockHealth; }
    void updateBlock() { setHealth(blockHealth / 2); }
    void updateParry() { parryState--;}
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
