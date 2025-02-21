using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public EnemyStats ES;
    public Transform[] waypoints; // Array to hold the waypoints

    private int currentWaypointIndex = 0; // Index of the current waypoint
    private float waitCounter = 0f; // Counter for waiting
    private bool isWaiting = false; // Flag to check if the enemy is waiting

    void Update()
    {   
        if (waypoints.Length == 0) return; // If no waypoints, do nothing

        if (isWaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= ES.idleTime)
            {
                isWaiting = false;
                waitCounter = 0f;
            }
            return;
        }
    }

    public void Patrol()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, ES.MovSpeed * Time.deltaTime);

        // Check if the enemy has reached the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Move to the next waypoint
            isWaiting = true; // Start waiting
        }
    }
}