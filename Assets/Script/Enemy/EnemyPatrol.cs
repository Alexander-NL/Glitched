using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public EnemyStats ES; // Reference to the enemy stats (movement speed, idle time, etc.)
    public EnemyChase EC;
    public Transform[] waypoints; // Array to hold the waypoints

    private int currentWaypointIndex = 0; // Index of the current waypoint
    private float waitCounter = 0f; // Counter for waiting
    public bool isWaiting = false; // Flag to check if the enemy is waiting
    public bool canPatrol;

    void Update()
    {
        if (waypoints.Length == 0) return; // If no waypoints, do nothing
        if (!EC.isMoving) return;

        if (isWaiting)
        {
            // Increment the wait counter
            waitCounter += Time.deltaTime;
            // Check if the waiting time is over
            if (waitCounter >= ES.idleTime)
            {
                isWaiting = false; // Stop waiting
                waitCounter = 0f; // Reset the counter
                MoveToNextWaypoint(); // Move to the next waypoint
            }
        }
        else
        {
            // Move towards the current waypoint
            Patrol();
        }
    }

    public void Patrol()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Move towards the target waypoint
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, ES.MovSpeed * Time.deltaTime);

        // Check if the enemy has reached the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Start waiting at the waypoint
            isWaiting = true;
        }
    }

    void MoveToNextWaypoint()
    {
        // Move to the next waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    // Public function to check if the enemy is currently waiting at a waypoint
    public bool IsWaiting()
    {
        return isWaiting;
    }

    // Public function to check if the enemy is currently patrolling (not waiting)
    public bool IsPatrolling()
    {
        return !isWaiting;
    }

    // Public function to check if the enemy has reached the last waypoint
    public bool HasReachedLastWaypoint()
    {
        return currentWaypointIndex == waypoints.Length - 1;
    }

    public bool CanPatrol()
    {
        return canPatrol;
    }
}