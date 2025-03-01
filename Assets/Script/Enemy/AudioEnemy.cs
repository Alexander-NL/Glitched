using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnemy : MonoBehaviour
{
    public AudioSource src;
    public AudioClip Damaged, Attack, Dead, Stunned;

    public void DamagedSound()
    {
        src.clip = Damaged;
        src.Play();
    }

    public void AttackSound()
    {
        src.clip = Attack;
        src.Play();
    }

    public void DeadSound()
    {
        src.clip = Dead;
        src.Play();
    }

    public void StunnedPlay()
    {
        src.clip = Stunned;
        src.Play();
    }
}
