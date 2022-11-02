using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SkeletonAI : MonoBehaviour, Unit //This AI uses a finite state machine to iterate between its states given the player that it can find
{   
    Animator animator;
    state currentState;
    Transform player;
    Seeker seeker;
    float damage = 10.0f, health = 100.0f, attackCD = 3f, knockBack = 15f;
    float time;
    public bool attackEnd = false, canAttack = true, aiTriggerd = false; //TODO make these private and add methods to access them 
    
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); 
        seeker = GetComponent<Seeker>();
        animator = GetComponent<Animator>();
        if (players[0] != null) //If there are no players for whatever reason the AI will just not create a state machine and stand there
        {
            player = players[0].transform;
            currentState = new idleState(this.gameObject, seeker, animator, player);
        }
        
    }

    void Update()
    {
        time += Time.deltaTime; //This just checks if the time between attack is less than 3 seconds an 
        if (time >= attackCD) { time = time - attackCD; canAttack = true; }
        if(player != null)
            currentState = currentState.Process(); //This just makes the state machine do its next process 
    }

    public void endAttack() { attackEnd = !attackEnd; } //This method is only called when the attack ends to let the state machine know the attack finished
    public void enableHitBox() { //This method just switches the hitbox on and off, only used during the animation to turn the axe attack box on and off
        PolygonCollider2D attackBox = GetComponent<PolygonCollider2D>(); 
        attackBox.enabled = !attackBox.isActiveAndEnabled;
        time = 0; //this ensures the time between attacks is 3 seconds by setting time to 0 during the attack
    }
    public float getDamage() { return damage; }
    private void OnTriggerEnter2D(Collider2D col) //This decrements the HP of the ai, and generates a hit effect | TODO put this into a helper method and add damage calculation based upon damage type
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

    public void takeDamage(float damage) //takes damage and if health is less than or equal to 0 it starts the death animation/sequence
    { 
        health -= damage;
        aiTriggerd = true;
        if( health <= 0)
        {
            currentState = new deathState(this.gameObject, seeker, animator, player);
        }
    }
    public float getKnockBack() { return knockBack; }
    public void cannotAttack() { canAttack = false; } //This triggers when the attack animation plays so that the AI cannot attack again until the cd has reached its end
    public void replace() //This method is run through the animator and it creates a prefab after the animation of death ends and destroys the skeleton AI
    {
        GameObject corpse = Instantiate(Resources.Load("skeletonCorpse")) as GameObject;
        corpse.transform.position = transform.position;
        corpse.transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
