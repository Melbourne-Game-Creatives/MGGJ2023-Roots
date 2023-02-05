using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExclamation : MonoBehaviour
{
    public AudioClip[] Exclamations;
    public AudioClip[] DeathSounds;
    public AudioSource audioSource;

    public void PlayExclamation()
    {
        var soundIndex = Random.Range(0, Exclamations.Length);

        audioSource.clip = Exclamations[soundIndex];
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        var soundIndex = Random.Range(0, DeathSounds.Length);

        audioSource.clip = DeathSounds[soundIndex];
        audioSource.Play();
    }
}
