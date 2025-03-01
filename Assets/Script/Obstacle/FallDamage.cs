using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    public Stats playerStats;  // Reference to the RespawnScript
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player fell down");

            if (playerStats != null)
            {
                // If the RespawnScript is found, you can call its methods
                playerStats.Die();  // Example: Call the Respawn method
            }
            else
            {
                Debug.LogWarning("RespawnScript not found on the Player GameObject.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
