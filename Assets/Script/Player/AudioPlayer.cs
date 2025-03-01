using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip DashSound, 
        FootstepSound, 
        HealSound, 
        PlayerDamagedSound, 
        PlayerDeadSound, 
        ShootSound, 
        BasicAttack1, 
        BasicAttack2, 
        BasicAttack3,
        ChargedRampUp,
        ChargedAttack1,
        ChargedAttack2,
        ChargedAttack3;
    
    public void DashSoundPlay()
    {
        src.clip = DashSound;
        src.Play();
    }

    public void FootstepSoundPlay()
    {
        src.clip = FootstepSound;
        src.Play();
    }

    public void HealSoundPlay()
    {
        src.clip = HealSound;
        src.Play();
    }

    public void PlayerDamagedSoundPlay()
    {
        src.clip = PlayerDamagedSound;
        src.Play();
    }

    public void PlayerDeadSoundPlay()
    {
        src.clip = PlayerDeadSound;
        src.Play();
    }

    public void ShootSoundPlay()
    {
        src.clip = ShootSound;
        src.Play();
    }

    public void BasicAttack1Play()
    {
        src.clip = BasicAttack1;
        src.Play();
    }

    public void BasicAttack2Play()
    {
        src.clip = BasicAttack2;
        src.Play();
    }

    public void BasicAttack3Play()
    {
        src.clip = BasicAttack3;
        src.Play();
    }

    public void ChargedRampUpPlay()
    {
        src.clip = ChargedRampUp;
        src.Play();
    }

    public void ChargedAttack1Play()
    {
        src.clip = ChargedAttack1;
        src.Play();
    }

    public void ChargedAttack2Play()
    {
        src.clip = ChargedAttack2;
        src.Play();
    }

    public void ChargedAttack3Play()
    {
        src.clip = ChargedAttack3;
        src.Play();
    }
}
