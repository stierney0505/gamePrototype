using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface spell // Abstract class for the spells controlled by the player, Currently WIP
{
    
    public Vector3 getVector();

    public void end(); //This method is for ending the flight/travel states of projectile spells
    public void remove(); //This methods removes the spell from the game, destroys it
    public float getDamage(); //Currently, this method should only be called to get the damage of a spell during a collison
    public char getType();
    public float getKnockBack();


}
