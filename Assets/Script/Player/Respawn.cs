using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public Vector3 lastRespawnPoint;  // Stores the last campfire position
    public GameObject player; // Reference to the player object
    private Stats playerStats;

    private void Start()
    {
        lastRespawnPoint = player.transform.position; // Default spawn point
        playerStats = player.GetComponent<Stats>();
    }

    public void UpdateRespawnPoint(Vector3 newRespawnPoint)
    {
        lastRespawnPoint = newRespawnPoint;
        Debug.Log("Respawn point updated: " + lastRespawnPoint);
    }

    public void RespawnPlayer()
    {
        player.transform.position = lastRespawnPoint;
        playerStats.HealPlayer();
        player.SetActive(true);
        Debug.Log("Player respawned at: " + lastRespawnPoint);
    }
}
