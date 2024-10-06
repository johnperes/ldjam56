using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip deathClip;
    public AudioClip nextLevelClip;
    public AudioClip shrinkClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayDeath()
    {
        audioSource.PlayOneShot(deathClip);
    }

    public void PlayNextLevel()
    {
        audioSource.PlayOneShot(nextLevelClip);
    }
    public void PlayShrink()
    {
        audioSource.PlayOneShot(shrinkClip);
    }
}

