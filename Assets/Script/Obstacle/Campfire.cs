using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public Stats S;  // Reference to the player's stats
    public RespawnScript R; // Reference to the RespawnScript

    private bool isPlayerInRange = false;  // Flag to check if the player is in range

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player is in range of the campfire.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player is out of range of the campfire.");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            HealPlayer();
            SetCheckPoint();
        }
    }

    public void HealPlayer()
    {
        S.CurrHP = S.MaxHP;
        S.HealPackAmmount = S.HealPackMax;
        S.M2_Ammo = S.M2_AmmoMax;
        Debug.Log("Player has been healed.");
    }

    public void SetCheckPoint()
    {
        if (R != null)
        {
            S.HasRespawn = true;
            R.UpdateRespawnPoint(transform.position);
            Debug.Log("Checkpoint set at campfire.");
        }
    }
}
