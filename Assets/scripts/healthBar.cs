using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    public static float totalHealth;
    public static float currentHealth;
    private static Image healthImage;
    // Start is called before the first frame update
    void Start()
    {
        healthImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setHealthBarValue(float value) { totalHealth = value; currentHealth = totalHealth; }

    public static void incrementPlayerHealth(float value)
    {
        currentHealth += value;
        healthImage.fillAmount = (currentHealth/totalHealth);   
    }
}
