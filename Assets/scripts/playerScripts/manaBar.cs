using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manaBar : MonoBehaviour
{
    private static float totalMana;
    private static float currentMana;
    private static Image healthImage;
    // Start is called before the first frame update
    void Start()
    {
        healthImage = GetComponent<Image>();
        healthImage.color = new Color(0.204f, 0.922f, 0.6f, 1);
    }

    public static void setManaBarValue(float value) { totalMana = value; currentMana = totalMana; }
    public static float getManaBarValue() { return currentMana; }
    public static void incrementPlayerMana(float value)
    {
        currentMana += value;
        healthImage.fillAmount = (currentMana / totalMana);
    }
}
