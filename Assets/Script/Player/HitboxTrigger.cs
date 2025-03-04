using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    public Stats S; // Reference to the Attack script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.CompareTag("Enemy"))
        {
            // Increase M2_Ammo in the Stats script
            S.AmmoIncrease();
        }
    }
}