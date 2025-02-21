using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("HP Settings")]
    public float CurrHP = 45f;
    public float MaxHP = 45f;

    [Header("Damage Settings")]
    public float Damage = 20f;
    public float Range = 1.2f;
    public float attackCooldown = 1f; //Time between Chrage attack
    public float WindUpTime = 1.2f;

    [Header("Speed Settings")]
    public float MovSpeed = 2f;
    public int idleTime = 2;

    [Header("Chase Settings")]
    public float chaseRange = 5f;
    public float chaseSpeed = 3f;

    private GameObject player;
    private Stats S;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        S = player.GetComponent<Stats>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            CurrHP = CurrHP - S.M2_Damage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Dead");
        gameObject.SetActive(false);
    }
}
