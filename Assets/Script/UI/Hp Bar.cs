using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Stats S; // Reference to the Player scripts
    public Image healthBarImage; // Reference to the UI Image component
    void Update()
    {
        if (S != null && healthBarImage != null)
        {
            // Calculate the fill amount based on the player's current HP
            float fillAmount = (float)S.CurrHP / S.MaxHP;
            healthBarImage.fillAmount = fillAmount;
        }
    }
}