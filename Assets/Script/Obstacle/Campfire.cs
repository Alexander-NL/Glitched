using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public Stats S;  // Reference to the player's stats
    public RespawnScript R; // Reference to the RespawnScript

    public AudioSource src;
    public AudioClip Interact;
    public bool FirstSpawn;

    private bool isPlayerInRange = false;  // Flag to check if the player is in range

    private void Start()
    {
        if(FirstSpawn == true)
        {
            SetCheckPoint();
        }
    }
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
            InteractPlay();
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

    public void InteractPlay()
    {
        src.clip = Interact;
        src.Play();
    }
}
