using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface spell // Abstract class for the spells controlled by the player, Currently WIP
{

    public void end(bool environment); //This method is for ending the flight/travel states of projectile spells and the bool checks if its due to the environment and if so should remove it immediately
    public void remove(); //This methods removes the spell from the game, destroys it
    public float getDamage(); //Currently, this method should only be called to get the damage of a spell during a collison
    public char getType();
    public float getKnockBack();

    public void addEnemy(Transform enemyTransform); //This method will be for spells that apply constant movement effects, such as dark6, or the air spells
    //TODO MAYBE: Change the parameter to gameobject if I want to add a 'movement controlled' effect to the enemies, i.e. when they aren't damaged but they are movement impaired
}
