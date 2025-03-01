using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Heal : MonoBehaviour
{
    public AudioPlayer AP;
    public Stats S; //Put Stat.cs here
    public void OnHeal()
    {
        if(S.HealPackAmmount > 0)
        {
            if (S.CurrHP < S.MaxHP)
            {
                HealPlayer();
                S.HealPackAmmount--;
            }
            else
            {
                Debug.Log("HP full");
            }
        }
        else
        {
            Debug.Log("No Heal Pack");
        }
    }

    void HealPlayer()
    {
        AP.HealSoundPlay();
        S.CurrHP = S.CurrHP + 25;
        if(S.CurrHP > S.MaxHP)
        {
            S.CurrHP = S.MaxHP;
            Debug.Log("OverHeal Revert back HP to max");
        }
        else
        {
            Debug.Log("Healing done");
        }
        
    }
}
