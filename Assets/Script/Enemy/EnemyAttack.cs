using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Scripts Used
    private Stats playerStats;
    public EnemyPatrol EP;
    public EnemyChase EC;
    public EnemyStats ES;

    private Transform player; // Reference to the player
    private float lastAttackTime = 0f; // Time of the last attack
    private bool isWindingUp = false; // Flag to check if the enemy is in wind-up phase
    private float windUpStartTime = 0f; // Time when the wind-up phase started
    private bool isStunned = false; // Flag to check if the enemy is stunned

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    void Update()
    {
        if (isStunned) return; // Do nothing if stunned

        if (player == null) return; // If no player, do nothing

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is within attack range
        if (distanceToPlayer <= ES.Range)
        {
            EC.StopMovement();

            // Check if enough time has passed since the last attack
            if (Time.time - lastAttackTime >= ES.attackCooldown && !isWindingUp)
            {
                StartWindUp();
            }

            // Check if the wind-up phase is complete
            if (isWindingUp && Time.time - windUpStartTime >= ES.WindUpTime)
            {
                ExecuteAttack();
            }
        }
        else
        {
            // If the player moves out of range during wind-up, cancel the attack
            if (isWindingUp)
            {
                CancelWindUp();
            }

            EC.ResumeMovement();
        }
    }

    void StartWindUp()
    {
        isWindingUp = true; // Start the wind-up phase
        windUpStartTime = Time.time; // Record the start time of the wind-up phase
        Debug.Log("Enemy is winding up to attack...");

        // Optional: Add visual/audio cues for the wind-up phase (e.g., a glowing effect or sound)
    }

    void CancelWindUp()
    {
        isWindingUp = false; // Cancel the wind-up phase
        Debug.Log("Enemy canceled the attack because the player moved out of range.");
    }

    void ExecuteAttack()
    {
        isWindingUp = false; // End the wind-up phase
        lastAttackTime = Time.time; // Update the last attack time

        playerStats = player.GetComponent<Stats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(ES.Damage); // Deal damage to the player
            Debug.Log("Enemy attacked the player for " + ES.Damage + " damage.");
        }
        else
        {
            EP.Patrol(); // Fallback to patrolling if player stats are not found
        }
    }

    public void Stun(float duration)
    {
        if (isStunned) return; // Prevent multiple stuns

        StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        isStunned = true; // Set stunned flag
        CancelWindUp(); // Cancel any ongoing wind-up
        EC.StopMovement(); // Stop movement

        Debug.Log("Enemy is stunned for " + duration + " seconds.");

        yield return new WaitForSeconds(duration); // Wait for the stun duration

        isStunned = false; // Reset stunned flag
        Debug.Log("Enemy is no longer stunned.");
    }

    public bool IsWindingUp()
    {
        return isWindingUp; // Return whether the enemy is winding up
    }

    // Optional: Draw attack range in the Unity Editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ES.Range);
    }
}