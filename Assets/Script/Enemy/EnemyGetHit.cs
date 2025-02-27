using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHitMelee : MonoBehaviour
{
    public EnemyStats ES;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Hitbox" tag
        if (collision.CompareTag("HitboxBA"))
        {
            Debug.Log("Player has hit the enemy!");
            ES.DamageEnemyBA();
        } else if (collision.CompareTag("HitboxBA2"))
        {
            Debug.Log("Player has hit the enemy!");
            ES.DamageEnemyBA2();
        } else if (collision.CompareTag("HitboxBA3"))
        {
            Debug.Log("Player has hit the enemy!");
            ES.DamageEnemyBA3();
        }
        else if (collision.CompareTag("HitboxH1")){
            ES.DamageEnemyH1();
        } else if (collision.CompareTag("HitboxH2"))
        {
            ES.DamageEnemyH2();
        } else if (collision.CompareTag("HitboxH3"))
        {
            ES.DamageEnemyH3();
        }
    }
}
