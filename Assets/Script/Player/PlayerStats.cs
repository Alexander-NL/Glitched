using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("HP Settings")]
    public float CurrHP = 100f;
    public float MaxHP = 100f;
    public float HP_PlayerStunDuration = 0.5f;
    public int HealPackAmmount = 3;
    public int HealPackMax = 3;
    public int HP_Module = 1; // HP Level

    [Header("Heal Settings")]
    public int Heal = 3; // until 8
    public int Heal_Module = 0; // Heal Level
    public bool HasRespawn = false;

    [Header("Sword Attack Settings")]
    public float M1_Range = 5f;
    public float M1_Damage = 10f;
    public float M1_pity;
    public float M1_ChargeTime = 1f;
    public int M1_Module = 0; // Basic Attack Level
    public float M1_Delay = 0.4f;
    public int M1_ComboMax = 3;

    [Header("Charged Attack Settings")]
    public float C_Damage;
    public float C_ChargeTime = 1f;
    // Level 1 2X M1 damage
    // Level 2 1.5X Level 1
    // Level 3 2X Level 1

    [Header("Shoot Settings")]
    public float M2_Damage = 5f;
    public int M2_Ammo = 3;
    public int M2_AmmoMax = 3;
    public int M2_Module = 0; // Shoot Level

    [Header("Parry Settings")]
    public float P_EnemyStunDuration = 1.5f;
    public int P_Module = 0; // Daze module (Parry Module)

    public RespawnScript R; // Reference to the RespawnScript
    public Movement M;

    void Start()
    {

    }

    void Update()
    {
        if (CurrHP <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        CurrHP -= damage;
        if (CurrHP <= 0)
        {
            CurrHP = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player has died.");
        gameObject.SetActive(false); // Temporarily disables the player object

        // Call Respawn after 3 seconds
        if(HasRespawn == true)
        {
            Invoke(nameof(Respawn), 3f);
        }
        Debug.Log("Player didnt save, Game Over.");
    }

    private void Respawn()
    {
       R.RespawnPlayer();
    }

    public void HealPlayer()
    {
        CurrHP = MaxHP;
        HealPackAmmount = HealPackMax;
        M2_Ammo = M2_AmmoMax;
        //M.Reset();
        Debug.Log("Player has been healed.");
    }
}
