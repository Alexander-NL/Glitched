using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    private Stats playerStats;  // Reference to the RespawnScript

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player fell down");

            // Try to get the RespawnScript component from the player
            playerStats = collision.GetComponent<Stats>();

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
        // You can add additional logic here if needed
    }
}