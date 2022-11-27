using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Unit //Unit base class for enemies WIP and incomplete
{   //Each unit should have a damage value and damage type, the type is a char either A,E,F,L, or W for Air Earth Fire Lightning or Water. The type will be used for damage calculation
    public float getDamage();
    public char getType();
    public float getKnockBack();
    public void takeDamage(spell spell);
    public void takeDamage(float damage);
    public void takeKnockBack(Vector2 dir, float knockBack);
}


