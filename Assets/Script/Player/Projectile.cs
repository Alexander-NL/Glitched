using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Self-destruction timer
    public float stunDuration = 1.5f; // Duration to stun the enemy

    void Start()
    {
        StartCoroutine(DestroyAfterTime()); // Start countdown
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the EnemyAttack component from the enemy
            EnemyAttack enemyAttack = collision.gameObject.GetComponent<EnemyAttack>();

            if (enemyAttack != null && enemyAttack.IsWindingUp())
            {
                // Stun the enemy
                enemyAttack.Stun(stunDuration);
            }
        }

        Destroy(gameObject); // Destroy the projectile
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject); // Destroy the projectile after lifetime expires
    }
}