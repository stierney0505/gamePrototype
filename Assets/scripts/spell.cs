using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class spell : MonoBehaviour
{
    
    public abstract Vector3 getVector();

    public abstract void end();
    
}
