using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class environmentStopScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<spell>(out spell spellComponent))
            spellComponent.end();
    }
}
