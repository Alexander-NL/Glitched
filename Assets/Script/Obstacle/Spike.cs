using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike: MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10; // Amount of damage to deal
    [SerializeField] private bool destroyOnCollision = true; // Destroy this object after collision

    public Stats S;  // Put Stat.cs here

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            S.CurrHP = S.CurrHP - damageAmount;
            Debug.Log("Player took " + damageAmount + " damage!");
            if (destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}