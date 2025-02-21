using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public EnemyPatrol EP;
    public EnemyStats ES;
    public Transform player;

    private bool isChasing = false; // Flag to check if the enemy is chasing the player
    private bool isMoving = true;

    void Update()
    {
        if (!isMoving) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is within chase range
        if (distanceToPlayer < ES.chaseRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            EP.Patrol();
        }
    }

    void ChasePlayer()
    {
        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, ES.chaseSpeed * Time.deltaTime);
    }

    // Optional: Draw chase range in the Unity Editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ES.chaseRange);
    }

    public void StopMovement()
    {
        isMoving = false; // Pause movement
    }

    public void ResumeMovement()
    {
        isMoving = true; // Resume movement
    }
}

