using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Unit //Unit base class for enemies WIP and incomplete
{
    public float getDamage();

    public float getKnockBack();
    public void takeDamage(float damage);
}


