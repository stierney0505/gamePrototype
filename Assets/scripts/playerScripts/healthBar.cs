using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    private static float totalHealth;
    private static float currentHealth;
    private static Image healthImage;
    // Start is called before the first frame update
    void Start()
    {
        healthImage = GetComponent<Image>();
        healthImage.color = new Color(0.204f, 0.922f, 0.6f, 1);
    }

    public static void setHealthBarValue(float value) { totalHealth = value; currentHealth = totalHealth; }

    public static void incrementPlayerHealth(float value)
    {
        currentHealth += value;
        healthImage.fillAmount = (currentHealth/totalHealth);
    }

    public static void healthBarColor() //This changes the health bar color based on the ratio between current health and total health
    { 
        if(currentHealth/totalHealth <= .80f) { healthImage.color = Color.green; }
        if(currentHealth/totalHealth <= .60f) { healthImage.color = Color.yellow; }
        if (currentHealth / totalHealth <= .40f) { healthImage.color = new Color(0.89f, 0.51f, 0.23f, 1); }
        if (currentHealth / totalHealth <= .20f) { healthImage.color = Color.red; }
    }
}
