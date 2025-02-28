using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyChase : MonoBehaviour
{
    public EnemyPatrol EP;
    public EnemyStats ES;
    public Transform player;

    private Vector2 previousPosition;
    private Vector2 Difference;
    public Animator Animator;

    private bool isChasing = false; // Flag to check if the enemy is chasing the player
    public bool isMoving = true;

    public bool NotAlive;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }
    void Update()
    {
        if (!isMoving) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is within chase range
        if (distanceToPlayer < ES.chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            EP.Patrol();
            CheckForFlip();
        }

        if (ES.CurrHP < 0)
        {
            NotAlive = true;
            isChasing = false;
            StopMovement();
        }
    }
    public Vector2 CalculateDirection()
    {
        // Example: Use mouse position to determine attack direction 
        Vector2 direction = (player.position - transform.position).normalized;
        return direction;
    }

    public int GetDirectionValue(Vector2 direction)
    {
        // Determine the direction index based on the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle >= 45 && angle < 135) // Up
            return 0;
        else if (angle >= -135 && angle < -45) // Down
            return 1;
        else
            return 2;
    }
    void CheckForFlip()
    {
        // Check the direction of movement based on the enemy's position
        if (transform.position.x < previousPosition.x)
        {
            // Enemy is moving left (-x direction), flip the sprite
            transform.localScale = new Vector3(-1, 1, 1); // Flip horizontally
        }
        else if (transform.position.x > previousPosition.x)
        {
            // Enemy is moving right (+x direction), unflip the sprite
            transform.localScale = new Vector3(1, 1, 1); // Reset to normal
        }
        previousPosition = transform.position;
    }
    public void ChasePlayer()
    {
        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, ES.chaseSpeed * Time.deltaTime);
        CheckForFlip();
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

    public bool FoundPlayer()
    {
        return NotAlive;
    }
}

