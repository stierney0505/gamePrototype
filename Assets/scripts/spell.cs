using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class spell : MonoBehaviour //Abstract class for the spells controlled by the player, Currently WIP
{
    
    public abstract Vector3 getVector();

    public abstract void end();
    
}
